using MongoDB.Bson;
using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {

        Task<List<Player>> GetPlayersForPaginatedSearchResultAsync(PlayerSearchByParametersModel model, FilterDefinition<Player> filter);

        Task<long> GetPlayersQueryCountAsync(FilterDefinition<Player> query);

        Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(string id);

        Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids);
    }
}
