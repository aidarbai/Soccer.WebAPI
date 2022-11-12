using Soccer.DAL.Models;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface ILeagueRepository : IGenericRepository<League>
    {
        Task<IEnumerable<League>> SearchByListOfLeagueNamesAsync(IEnumerable<string> leagueNames);
    }
}
