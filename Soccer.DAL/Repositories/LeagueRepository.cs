using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;
using static Soccer.COMMON.Constants.AppConstants;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Soccer.COMMON.ViewModels;

namespace Soccer.DAL.Repositories
{
    public class LeagueRepository : GenericRepository<League>, ILeagueRepository
    {
        private static readonly Dictionary<LeagueSortBy, Expression<Func<League, object>>> _dictionary = new() {
            { LeagueSortBy.NAME, x => x.Name},
            { LeagueSortBy.COUNTRY, x => x.Country}};
        public LeagueRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<List<League>> GetLeaguesForPaginatedSearchResultsAsync(LeagueSearchModel model, FilterDefinition<League> filter)
        {
            filter ??= Builders<League>.Filter.Empty;

            var filterDebug1 = filter.Render(_collection.DocumentSerializer, _collection.Settings.SerializerRegistry).ToJson();

            var registrySerializer = BsonSerializer.SerializerRegistry;
            var documentSerializer = registrySerializer.GetSerializer<League>();
            var rendered = filter.Render(documentSerializer, registrySerializer);
            var filterDebug2 = rendered.ToJson();

            var teams = await _collection
                                    .Find(filter)
                                    .Sort(GetSortDefinition(model))
                                    .Skip((int)model.PageNumber * (int)model.PageSize)
                                    .Limit((int)model.PageSize)
                                    .ToListAsync();

            return teams;
        }

        private static SortDefinition<League> GetSortDefinition(LeagueSearchModel model)
        {

            if (!_dictionary.ContainsKey(model.SortBy))
            {
                return Builders<League>.Sort.Ascending(t => t.Name);
            }

            if (model.Order == Order.ASC)
            {
                return Builders<League>.Sort.Ascending(_dictionary[model.SortBy]);
            }

            return Builders<League>.Sort.Descending(_dictionary[model.SortBy]);

        }

        public async Task<long> GetLeaguesQueryCountAsync(FilterDefinition<League> filter)
        {
            var count = await _collection.Find(filter).CountDocumentsAsync();

            return count;
        }

        public async Task<IEnumerable<League>> SearchByListOfLeagueNamesAsync(IEnumerable<string> leagueNames)
        {
            var filter = Builders<League>.Filter.In(l => l.Name, leagueNames);
            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            return matchedDocuments;
        }
    }
}
