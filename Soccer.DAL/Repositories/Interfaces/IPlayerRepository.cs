using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using System.Threading.Tasks;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<PaginatedResponse<Player>> GetTeamsPaginatedAsync(SortAndPagePlayerModel model);

        Task<IEnumerable<Player>> GetByTeamIdAsync(string id);

        Task<IEnumerable<Player>> SearchByListOfIdsAsync(IEnumerable<string> ids);

        Task<IEnumerable<Player>> SearchByNameAsync(string search);
    }
}
