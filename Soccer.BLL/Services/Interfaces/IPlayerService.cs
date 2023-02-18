using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Services.Interfaces
{
    public interface IPlayerService
    {
        Task CreateAsync(Player newPlayer);

        Task CreateManyAsync(IEnumerable<Player> newPlayers);

        Task UpdateAsync(Player updatedPlayer);

        Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids);

        Task<PaginatedResponse<PlayerVm>> SearchByParametersAsync(PlayerSearchByParametersModel searchModel);

        IEnumerable<Player> MapPlayerDTOListToPlayerList(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto, string leagueId);
    }
}