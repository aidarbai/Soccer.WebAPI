using AutoMapper;
using Microsoft.Extensions.Configuration;
using Soccer.BLL.DTOs;
using Soccer.DAL.Models;
using Microsoft.Extensions.Logging;
using Soccer.BLL.Services.Interfaces;

namespace Soccer.BLL.Services
{
    public class ImportService : IImportService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientService _dataDownloader;
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;
        private readonly ILogger<ImportService> _logger;
        public ImportService(
            IHttpClientService dataDownloader,
            ILeagueService leagueService,
            IConfiguration configuration,
            ITeamService teamService,
            IPlayerService playerService,
            IMapper mapper,
            ILogger<ImportService> logger)
        {
            _dataDownloader = dataDownloader;
            _leagueService = leagueService;
            _configuration = configuration;
            _teamService = teamService;
            _playerService = playerService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task ImportLeagueAsync()
        {
            string leagueId = _configuration["Football-API:LeagueId"];

            string url = string.Format(_configuration["Football-API:LeagueById"], leagueId);

            var result = await _dataDownloader.GetDataAsync<ResponseImportDTO<ResponseLeagueImportDTO>>(url);
            // TODO test mock getDataAsync returns 1) null 2) returns object -> check if Map and CreateAsync have been called (mock verify times)
            if (result?.Response!.Count > 0)
            {
                var leagueImportDto = result.Response!.FirstOrDefault();

                var newLeague = _mapper.Map<League>(leagueImportDto);

                await _leagueService.CreateAsync(newLeague);
            }
        }

        public async Task ImportTeamsByLeagueAsync()
        {

            string leagueId = _configuration["Football-API:LeagueId"];

            string url = string.Format(_configuration["Football-API:TeamsByLeagueId"], leagueId);

            var result = await _dataDownloader.GetDataAsync<ResponseImportDTO<ResponseTeamImportDTO>>(url);

            if (result?.Response!.Count > 0)
            {
                var responseTeamImportDto = result.Response;

                var teams = new List<Team>(responseTeamImportDto!.Count);

                foreach (var teamImportDto in responseTeamImportDto!)
                {
                    var team = _mapper.Map<Team>(teamImportDto, opt => opt.AfterMap((_, dest) => dest.League = leagueId));

                    teams.Add(team);
                }

                await _teamService.CreateManyAsync(teams);
            }
        }

        public async Task ImportAllPlayersByTeamsListAsync()
        {
            //var teamIds = await _teamService.GetTeamIdsAsync();

            var teamIds = new string[] { "529", "530" };

            foreach (var item in teamIds)
            {
                await ImportPlayersByTeamAsync(item);
                await Task.Delay(10000);
            }
        }

        private async Task ImportPlayersByTeamAsync(string teamId)
        {
            string leagueId = _configuration["Football-API:LeagueId"];
            string url = string.Format(_configuration["Football-API:PlayersByTeamId"], teamId, 1);

            var result = await _dataDownloader.GetDataAsync<ResponseImportDTO<ResponsePlayerImportDTO>>(url);
            var totalPages = result?.Paging?.Total;
            if (totalPages > 1)
            {
                for (int i = 2; i <= totalPages; i++)
                {
                    url = string.Format(_configuration["Football-API:PlayersByTeamId"], teamId, i);

                    var nextPageResult = await _dataDownloader.GetDataAsync<ResponseImportDTO<ResponsePlayerImportDTO>>(url);

                    if (nextPageResult?.Response!.Count > 0)
                        result?.Response?.AddRange(nextPageResult.Response);
                }
            }

            _logger.LogInformation("{count} playerDTOs downloaded for team {teamId}", result?.Response.Count, teamId);

            if (result?.Response!.Count > 0)
            {
                var players = _playerService.MapPlayerDTOListToPlayerList(result.Response, leagueId);

                _logger.LogInformation("{count} players mapped for team {teamId}", players.Count(), teamId);

                var playersIds = players?.Select(p => p.Id);

                var existingPlayers = await _playerService.GetPlayersByListOfIdsAsync(playersIds!);

                _logger.LogInformation("{count} existing players found for team {teamId}", existingPlayers.Count(), teamId);

                if (existingPlayers.Any())
                {

                    foreach (var existingPlayer in existingPlayers)
                    {
                        var newStat = players!.First(p => p.Id == existingPlayer.Id).Statistics;

                        var statToAdd = newStat.ExceptBy(existingPlayer.Statistics.Select(l => l.Team), x => x.Team);

                        if (statToAdd.Any())
                        {
                            existingPlayer.Statistics.AddRange(statToAdd);

                            await _playerService.UpdateAsync( existingPlayer);
                        }
                    }
                    
                    players = players!.ExceptBy(existingPlayers.Select(e => e.Id), p => p.Id);
                }

#pragma warning disable CS8604 // Possible null reference argument.
                if (players.Any())
                {
                    _logger.LogInformation("{count} players of team {teamId} sent to db", players!.Count(), teamId);

                    await _playerService.CreateManyAsync(players!);
                }
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }
        //public async Task ImportPlayerByIdAsync(string playerId)
        //{
        //    string url = string.Format(_configuration["Football-API:PlayerById"], playerId, 1);

        //    var result = await _dataDownloader.GetDataAsync<ResponseImportDTO<ResponsePlayerImportDTO>>(url);
        //    var totalPages = result?.Paging?.Total;
        //    if (totalPages > 1)
        //    {
        //        for (int i = 2; i <= totalPages; i++)
        //        {
        //            url = string.Format(_configuration["Football-API:PlayerById"], playerId, i);

        //            var nextPageResult = await _dataDownloader.GetDataAsync<ResponseImportDTO<ResponsePlayerImportDTO>>(url);

        //            if (nextPageResult?.Response!.Count > 0)
        //                result?.Response?.AddRange(nextPageResult.Response);
        //        }
        //    }

        //    if (result?.Response!.Count > 0)
        //    {
        //        var players = await _playerService.MapPlayerDTOListToPlayerList(result.Response);

        //        await _playerService.CreateManyAsync(players);
        //    }
        //}
    }
}
