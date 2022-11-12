using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.DAL.Repositories
{
    public class LeagueRepository : GenericRepository<League>, ILeagueRepository
    {

        public LeagueRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IEnumerable<League>> SearchByListOfLeagueNamesAsync(IEnumerable<string> leagueNames)
        {
            var filter = Builders<League>.Filter.In(l => l.Name, leagueNames);
            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            return matchedDocuments;
        }
    }
}
