using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soccer.BLL.DTOs;
using Soccer.BLL.MediatR.Commands;
using Soccer.BLL.Services;
using Soccer.BLL.Services.Interfaces;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers
{
    public class ImportPlayersHandler : IRequestHandler<ImportPlayersCommand, Unit>
    {
        private readonly IConfiguration _configuration;
        private readonly IPlayerRepository _repository;
        private readonly IHttpClientService _dataDownloader;
        private readonly IPlayerService _playerService;
        private readonly ILogger<ImportPlayersHandler> _logger;

        public ImportPlayersHandler(
            IConfiguration configuration,
            IPlayerRepository repository,
            IHttpClientService dataDownloader,
            IPlayerService playerService,
            ILogger<ImportPlayersHandler> logger)
        {
            _configuration = configuration;
            _repository = repository;
            _dataDownloader = dataDownloader;
            _playerService = playerService;
            _logger = logger;
            
        }

        public async Task<Unit> Handle(ImportPlayersCommand request, CancellationToken cancellationToken)
        {
            string teamId = request.Team; //TODO call importservice
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
                var players = _playerService.MapPlayerDTOListToPlayerList(result.Response, leagueId); //TODO ???

                _logger.LogInformation("{count} players mapped for team {teamId}", players.Count(), teamId);

                var playersIds = players?.Select(p => p.Id);

                var existingPlayers = await _playerService.GetPlayersByListOfIdsAsync(playersIds!); //TODO ???

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

                            await _playerService.UpdateAsync(existingPlayer);
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

            return Unit.Value;
        }
    }
}