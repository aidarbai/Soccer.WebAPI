using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<PaginatedResponse<Team>> GetTeamsPaginatedAsync(SortAndPageTeamModel model);

        Task<IEnumerable<Team>> SearchByNameAsync(string search);
    }
}
