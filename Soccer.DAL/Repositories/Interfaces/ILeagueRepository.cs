using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface ILeagueRepository : IGenericRepository<League>
    {
        Task<List<League>> GetTeamsForPaginatedSearchResultsAsync(LeagueSearchModel model, FilterDefinition<League> filter);

        Task<long> GetLeaguesQueryCountAsync(FilterDefinition<League> filter);
        Task<IEnumerable<League>> SearchByListOfLeagueNamesAsync(IEnumerable<string> leagueNames);
    }
}
