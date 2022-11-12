using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<IEnumerable<Player>> GetAllAsync();
        Task<PaginatedResponse<Player>> GetTeamsPaginateAsync(SortAndPagePlayerModel model);

        Task<Player> GetByIdAsync(string id);

        Task<IEnumerable<Player>> GetByTeamIdAsync(string id);
        Task CreateAsync(Player newPlayer);

        Task CreateManyAsync(IEnumerable<Player> newPlayers);

        Task UpdateAsync(Player updatedPlayer);

        Task<IEnumerable<Player>> SearchByListOfIdsAsync(IEnumerable<string> ids);

        Task<IEnumerable<Player>> SearchByNameAsync(string search);

        IEnumerable<Player> MapPlayerDTOListToPlayerList(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto, string leagueId);
    }
}