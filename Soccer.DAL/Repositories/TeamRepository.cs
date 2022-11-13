using AutoMapper;
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
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        private readonly IMapper _mapper;
        private static readonly Dictionary<TeamSortBy, Expression<Func<Team, object>>> _dictionary = new() {
            { TeamSortBy.NAME, x => x.Name},
            { TeamSortBy.FOUNDED, x => x.Founded}};
        public TeamRepository(IConfiguration configuration, IMapper mapper) : base(configuration)
        {
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<TeamVm>> GetTeamsPaginatedAsync(SortAndPageTeamModel model)
        {
            var count = await _collection.EstimatedDocumentCountAsync();

            var teams = await _collection
                                .Find(Builders<Team>.Filter.Empty)
                                .Sort(GetSortDefinition(model))
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Limit(model.PageSize)
                                .ToListAsync();

            var result = new PaginatedResponse<TeamVm>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,

                Results= _mapper.Map<List<TeamVm>>(teams)
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
            if (search.First() == '*' || search.Last() == '*')
            {
                var builder = Builders<Team>.Filter;
                search = search.Replace("*", string.Empty);
                var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
                var filter = builder.Regex("Name", queryExpr) | builder.Regex("City", queryExpr);
                return await _collection.Find(filter).ToListAsync();
            }

            return await _collection.Find(t => t.Name == search).ToListAsync(); //TODO do we need an index?
        }
    }
}
