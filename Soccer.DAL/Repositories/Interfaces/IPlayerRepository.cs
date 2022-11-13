using MongoDB.Bson;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using System.Threading.Tasks;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<PaginatedResponse<PlayerVM>> GetPlayersPaginatedAsync(SortAndPagePlayerModel model);

        Task<List<Player>> GetPlayersForPaginatedResponseAsync(SortAndPagePlayerModel model, BsonDocument query);

        Task<long> GetPlayersCountAsync();

        Task<long> GetPlayersQueryCountAsync(BsonDocument query);

        Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(string id);

        Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids);

        Task<IEnumerable<Player>> SearchByNameAsync(string search);

        BsonDocument BuildQuery(int from, int to);
    }
}
