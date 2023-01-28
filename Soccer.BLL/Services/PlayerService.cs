using MongoDB.Driver;
using Soccer.COMMON.Helpers;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Helpers;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.Services
{
    public class PlayerService : IPlayerService
    {

        private readonly IPlayerRepository _repository;
        private readonly IMapper _mapper;
        public PlayerService(
            IPlayerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task CreateAsync(Player newPlayer) => await _repository.CreateAsync(newPlayer);

        public async Task CreateManyAsync(IEnumerable<Player> newPlayers)
        {
            await _repository.CreateManyAsync(newPlayers);
        }

        public async Task UpdateAsync(Player updatedPlayer) => await _repository.UpdateAsync(updatedPlayer);

        public async Task RemoveAsync(string id) => await _repository.RemoveAsync(id);

        public async Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids) => await _repository.GetPlayersByListOfIdsAsync(ids);

        public async Task<PaginatedResponse<PlayerVM>> SearchByParametersAsync(PlayerSearchByParametersModel searchModel)
        {
            DateHelperForSearchModel.ProcessAgeAndDates(searchModel);

            var filter = FilterBuilder.Build(searchModel);

            long count = await _repository.GetPlayersQueryCountAsync(filter);
            
            int totalPages = (int)Math.Ceiling(decimal.Divide(count, searchModel.PageSize));

            if (searchModel.PageNumber > totalPages)
            {
                return new PaginatedResponse<PlayerVM>
                {
                    PageSize = searchModel.PageSize,
                    PageNumber = searchModel.PageNumber + 1,
                    TotalPages = totalPages,
                };
            }

            var players = await _repository.GetPlayersForPaginatedSearchResultAsync(searchModel, filter);

            var result = new PaginatedResponse<PlayerVM>
            {
                ItemsCount = count,
                PageSize = searchModel.PageSize,
                TotalPages = totalPages,
                PageNumber = searchModel.PageNumber + 1,
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
