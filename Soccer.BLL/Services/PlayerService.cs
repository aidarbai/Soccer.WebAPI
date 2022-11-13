﻿using AutoMapper;
using MongoDB.Driver;
using Newtonsoft.Json;
using Soccer.BLL.DTOs;
using Soccer.BLL.Services.Interfaces;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.Services
{
    public class PlayerService : IPlayerService
    {

        private readonly IPlayerRepository _repository;
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;
        public PlayerService(
            IPlayerRepository repository,
            ILeagueService leagueService,
            ITeamService teamService,
            IMapper mapper)
        {
            _repository = repository;
            _leagueService = leagueService;
            _teamService = teamService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Player>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<PaginatedResponse<PlayerVM>> GetPlayersPaginatedAsync(SortAndPagePlayerModel model)
        {
            var count = await _repository.GetPlayersCountAsync();

            var players = await _repository.GetPlayersForPaginatedResponseAsync(model, null!);

            var result = new PaginatedResponse<PlayerVM>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,
                Results = _mapper.Map<List<PlayerVM>>(players)
            };

            return result;
        }

        public async Task<Player> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(string id) => await _repository.GetPlayersByTeamIdAsync(id);
        public async Task CreateAsync(Player newPlayer) => await _repository.CreateAsync(newPlayer);

        public async Task CreateManyAsync(IEnumerable<Player> newPlayers)
        {
            await _repository.CreateManyAsync(newPlayers);
        }

        public async Task UpdateAsync(Player updatedPlayer) => await _repository.UpdateAsync(updatedPlayer);

        public async Task RemoveAsync(string id) => await _repository.RemoveAsync(id);

        public async Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids) => await _repository.GetPlayersByListOfIdsAsync(ids);
        public async Task<IEnumerable<Player>> SearchByNameAsync(string search) => await _repository.SearchByNameAsync(search);

        public async Task<PaginatedResponse<PlayerVM>> SearchByAgeAsync(int from, int to, SortAndPagePlayerModel model) // TODO implement find model
        {
            var query = _repository.BuildQuery(from, to);

            long count = await _repository.GetPlayersQueryCountAsync(query);

            var players = await _repository.GetPlayersForPaginatedResponseAsync(model, query);

            var result = new PaginatedResponse<PlayerVM> // TODO move to separate method
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,
                Results = _mapper.Map<List<PlayerVM>>(players)
            };

            return result;
        }

        public IEnumerable<Player> MapPlayerDTOListToPlayerList(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto, string leagueId)
        {
            var playerList = FilterPlayerDTOList(responsePlayerImportDto, leagueId);

            var players = _mapper.Map<IEnumerable<Player>>(playerList);

            return players;
        }

        private static IEnumerable<ResponsePlayerImportDTO> FilterPlayerDTOList(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto, string leagueId)
        {
            foreach (var playerImportDto in responsePlayerImportDto)
            {
                var playerStatistics = playerImportDto.Statistics
                    .Where(s => s.League.Id.ToString() == leagueId);

                if (playerStatistics.Count() > 1)
                {
                    playerStatistics = playerStatistics.Where(s => s.Games.Appearences != 0);
                }

                playerImportDto.Statistics = playerStatistics.ToList();
            }

            responsePlayerImportDto = responsePlayerImportDto.Where(r => r.Statistics.Count > 0);

            return responsePlayerImportDto;
        }
    }
}
