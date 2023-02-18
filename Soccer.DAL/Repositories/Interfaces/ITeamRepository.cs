using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Task<List<Team>> GetTeamsForPaginatedSearchResultsAsync(TeamSearchModel model, FilterDefinition<Team> filter);

        Task<long> GetTeamsQueryCountAsync(FilterDefinition<Team> filter);
    }
}
