using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Soccer.COMMON.Constants.AppConstants;

namespace Soccer.DAL.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private static readonly Dictionary<PlayerSortBy, Expression<Func<Player, object>>> _dictionary = new Dictionary<PlayerSortBy, Expression<Func<Player, object>>>() {
            { PlayerSortBy.FIRSTNAME, x => x.Firstname},
            { PlayerSortBy.LASTNAME, x => x.Lastname}};
        public PlayerRepository(IConfiguration configuration) : base(configuration)
        {

        }
        
        public async Task<PaginatedResponse<Player>> GetTeamsPaginatedAsync(SortAndPagePlayerModel model)
        {
            var count = await _collection.EstimatedDocumentCountAsync();

            var result = new PaginatedResponse<Player>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,

                Results = await _collection
                                .Find(Builders<Player>.Filter.Empty)
                                .Sort(GetSortDefinition(model))
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Limit(model.PageSize)
                                .ToListAsync()
            };

            return result;
        }

        private static SortDefinition<Player> GetSortDefinition(SortAndPagePlayerModel model)
        {

            if (!_dictionary.ContainsKey(model.SortBy))
            {
                return Builders<Player>.Sort.Ascending(t => t.Name);
            }

            if (model.Order == Order.ASC)
            {
                return Builders<Player>.Sort.Ascending(_dictionary[model.SortBy]);
            }

            return Builders<Player>.Sort.Descending(_dictionary[model.SortBy]);

        }

        public async Task<IEnumerable<Player>> GetByTeamIdAsync(string id)
        {
            var matchedDocuments = await _collection.Find(c => c.Statistics.Any(s => s.Team == id)).ToListAsync();

            return matchedDocuments;
        }

        public async Task<IEnumerable<Player>> SearchByListOfIdsAsync(IEnumerable<string> ids)
        {
            var filter = Builders<Player>.Filter.In(l => l.Id, ids);
            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            return matchedDocuments;
        }

        public async Task<IEnumerable<Player>> SearchByNameAsync(string search)
        {
            var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
            var builder = Builders<Player>.Filter;
            var filter = builder.Regex("Firstname", queryExpr) | builder.Regex("Lastname", queryExpr);

            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            //var matchedDocuments = await _teamCollection.Find(t =>
            //t.Name.ToLower().Contains(search.ToLower()) ||
            //t.City.ToLower().Contains(search.ToLower()))
            //.ToListAsync();

            //TODO regex engages when the last char is * else exact search
            //filter gte or lte by date of birth and range
            //search by bool (capitanes)
            //filter in and match (array)
            //static extension (if field is not null, add query with filter)
            //check indexes in Mongo and add it to collection


            return matchedDocuments;
        }
    }
}
