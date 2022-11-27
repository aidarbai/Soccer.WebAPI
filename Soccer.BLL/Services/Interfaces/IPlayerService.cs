using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Services.Interfaces
{
    public interface IPlayerService
    {
        //Task<PaginatedResponse<PlayerVM>> GetPlayersPaginatedAsync(SortAndPagePlayerModel model);

        Task<Player> GetByIdAsync(string id);

        Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(string id);
        Task CreateAsync(Player newPlayer);

        Task CreateManyAsync(IEnumerable<Player> newPlayers);

        Task UpdateAsync(Player updatedPlayer);

        Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids);

        Task<IEnumerable<Player>> SearchByNameAsync(string search);

        //Task<PaginatedResponse<PlayerVM>> SearchByAgeAsync(int from, int to, SortAndPagePlayerModel model);

        //Task<PaginatedResponse<PlayerVM>> SearchByDateOfBirthAsync(string from, string to, SortAndPagePlayerModel model);

        Task<PaginatedResponse<PlayerVM>> SearchByParametersAsync(PlayerSearchByParametersModel searchModel);

        IEnumerable<Player> MapPlayerDTOListToPlayerList(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto, string leagueId);
    }
}