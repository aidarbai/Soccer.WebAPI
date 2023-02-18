using AutoMapper;
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

        public async Task<List<Team>> GetTeamsForPaginatedSearchResultsAsync(TeamSearchModel model, FilterDefinition<Team> filter)
        {
            filter ??= Builders<Team>.Filter.Empty;

            var filterDebug1 = filter.Render(_collection.DocumentSerializer, _collection.Settings.SerializerRegistry).ToJson();

            var registrySerializer = BsonSerializer.SerializerRegistry;
            var documentSerializer = registrySerializer.GetSerializer<Team>();
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

        private static SortDefinition<Team> GetSortDefinition(TeamSearchModel model)
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

        public async Task<long> GetTeamsQueryCountAsync(FilterDefinition<Team> filter)
        {
            var count = await _collection.Find(filter).CountDocumentsAsync();

            return count;
        }
    }
}
