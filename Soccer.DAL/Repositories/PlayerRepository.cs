using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;
using System.Text.RegularExpressions;
using static Soccer.COMMON.Constants.AppConstants;

namespace Soccer.DAL.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private static readonly Dictionary<PlayerSortBy, string> _sortByFieldsDictionary = new()
        {
            { PlayerSortBy.FIRSTNAME, "Firstname"},
            { PlayerSortBy.LASTNAME, "Lastname"},
            { PlayerSortBy.AGE,"Birth.Date"},
            { PlayerSortBy.GAMESPLAYED, "Statistics.Games.Appearences"}
        };
        public PlayerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<Player>> GetPlayersForPaginatedSearchResultAsync(PlayerSearchByParametersModel model, FilterDefinition<Player> filter)
        {
            filter ??= Builders<Player>.Filter.Empty;

            var filterDebug1 = filter.Render(_collection.DocumentSerializer, _collection.Settings.SerializerRegistry).ToJson();

            var registrySerializer = BsonSerializer.SerializerRegistry;
            var documentSerializer = registrySerializer.GetSerializer<Player>();
            var rendered = filter.Render(documentSerializer, registrySerializer);
            var filterDebug2 = rendered.ToJson();

            var players = await _collection
                                    .Find(filter)
                                    .Sort(GetSortDefinitionForSearchModel(model))
                                    .Skip((int)model.PageNumber * (int)model.PageSize)
                                    .Limit((int)model.PageSize)
                                    .ToListAsync();

            return players;
        }

        private static SortDefinition<Player> GetSortDefinitionForSearchModel(PlayerSearchByParametersModel model)
        {

            if (!_sortByFieldsDictionary.ContainsKey(model.SortBy))
            {
                return Builders<Player>.Sort.Ascending(t => t.Name);
            }

            if (model.SortBy == PlayerSortBy.AGE)
            {
                return model.Order == Order.ASC? 
                                      Builders<Player>.Sort.Descending(_sortByFieldsDictionary[model.SortBy]) :
                                      Builders<Player>.Sort.Ascending(_sortByFieldsDictionary[model.SortBy]);
            }

            if (model.Order == Order.ASC)
            {
                return Builders<Player>.Sort.Ascending(_sortByFieldsDictionary[model.SortBy]);
            }

            return Builders<Player>.Sort.Descending(_sortByFieldsDictionary[model.SortBy]);
        }

        public async Task<long> GetPlayersQueryCountAsync(FilterDefinition<Player> filter)
        {
            var count = await _collection.Find(filter).CountDocumentsAsync();

            return count;
        }

        public async Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(string teamId)
        {
            var filter = Builders<Player>.Filter.ElemMatch(x => x.Statistics, y => y.Team == teamId);
            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            return matchedDocuments;
        }

        public async Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids)
        {
            var filter = Builders<Player>.Filter.In(l => l.Id, ids);
            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            return matchedDocuments;
        }
    }
}
