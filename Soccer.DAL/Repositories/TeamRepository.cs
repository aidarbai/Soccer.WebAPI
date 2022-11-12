using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        private static readonly Dictionary<TeamSortBy, Expression<Func<Team, object>>> _dictionary = new() {
            { TeamSortBy.NAME, x => x.Name},
            { TeamSortBy.FOUNDED, x => x.Founded}};
        public TeamRepository(IConfiguration configuration) : base(configuration)
        {

        }
        
        public async Task<PaginatedResponse<Team>> GetTeamsPaginatedAsync(SortAndPageTeamModel model)
        {
            var count = await _collection.EstimatedDocumentCountAsync();

            var result = new PaginatedResponse<Team>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,

                Results = await _collection
                                .Find(Builders<Team>.Filter.Empty)
                                .Sort(GetSortDefinition(model))
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Limit(model.PageSize)
                                .ToListAsync()
            };

            return result;
        }

        private static SortDefinition<Team> GetSortDefinition(SortAndPageTeamModel model)
        {

            if (!_dictionary.ContainsKey(model.SortBy))
            {
                return Builders<Team>.Sort.Ascending(t => t.Name);
            }

            if (model.Order == Order.ASC)
            {
                return Builders<Team>.Sort.Ascending(_dictionary[model.SortBy]);
            }

            return Builders<Team>.Sort.Descending(_dictionary[model.SortBy]);

        }

        public async Task<IEnumerable<Team>> SearchByNameAsync(string search)
        {
            var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
            var builder = Builders<Team>.Filter;
            var filter = builder.Regex("Name", queryExpr) | builder.Regex("City", queryExpr);

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
