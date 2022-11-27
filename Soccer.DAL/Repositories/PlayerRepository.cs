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
        private readonly IMapper _mapper;

        private static readonly Dictionary<PlayerSortBy, string> _dictionary = new()
        {
            { PlayerSortBy.FIRSTNAME, "Firstname"},
            { PlayerSortBy.LASTNAME, "Lastname"},
            { PlayerSortBy.AGE,"Birth.Date"},
            { PlayerSortBy.GAMESPLAYED, "Statistics.Games.Appearences"}
        };
        public PlayerRepository(IConfiguration configuration, IMapper mapper) : base(configuration)
        {
            _mapper = mapper;
        }

        //public async Task<List<Player>> GetPlayersForPaginatedResponseAsync(SortAndPagePlayerModel model, FilterDefinition<Player> filter)
        //{
        //    filter ??= Builders<Player>.Filter.Empty;

        //    var players = await _collection
        //                            .Find(filter)
        //                            .Sort(GetSortDefinition(model))
        //                            .Skip((int)(model.PageNumber - 1) * (int)model.PageSize)
        //                            .Limit((int)model.PageSize)
        //                            .ToListAsync();

        //    return players;
        //}

        //private static SortDefinition<Player> GetSortDefinition(SortAndPagePlayerModel model)
        //{

        //    if (!_dictionary.ContainsKey(model.SortBy))
        //    {
        //        return Builders<Player>.Sort.Ascending(t => t.Name);
        //    }

        //    if (model.Order == Order.ASC)
        //    {
        //        return Builders<Player>.Sort.Ascending(_dictionary[model.SortBy]);
        //    }

        //    return Builders<Player>.Sort.Descending(_dictionary[model.SortBy]);

        //}

        public async Task<List<Player>> GetPlayersForPaginatedSearchResultAsync(PlayerSearchByParametersModel model, FilterDefinition<Player> filter)
        {
            filter ??= Builders<Player>.Filter.Empty;

            var players = await _collection
                                    .Find(filter)
                                    .Sort(GetSortDefinitionForSearchModel(model))
                                    .Skip((int)(model.PageNumber - 1) * (int)model.PageSize)
                                    .Limit((int)model.PageSize)
                                    .ToListAsync();

            return players;
        }

        private static SortDefinition<Player> GetSortDefinitionForSearchModel(PlayerSearchByParametersModel model)
        {

            if (!_dictionary.ContainsKey(model.SortBy))
            {
                return Builders<Player>.Sort.Ascending(t => t.Name);
            }

            if (model.Order == Order.ASC && model.SortBy == PlayerSortBy.AGE) //TODO ???
            {
                return Builders<Player>.Sort.Descending(_dictionary[model.SortBy]);
            }

            if (model.Order == Order.DESC && model.SortBy == PlayerSortBy.AGE)
            {
                return Builders<Player>.Sort.Ascending(_dictionary[model.SortBy]);
            }

            if (model.Order == Order.ASC)
            {
                return Builders<Player>.Sort.Ascending(_dictionary[model.SortBy]);
            }

            return Builders<Player>.Sort.Descending(_dictionary[model.SortBy]);

            //return Builders<Player>.Sort.Ascending("Birth.Date");

        }

        public async Task<long> GetPlayersCountAsync()
        {
            var count = await _collection.EstimatedDocumentCountAsync();

            return count;
        }

        public async Task<long> GetPlayersQueryCountAsync(FilterDefinition<Player> filter)
        {
            var count = await _collection.Find(filter).CountDocumentsAsync();

            return count;
        }


        public async Task<IEnumerable<Player>> GetPlayersByTeamIdAsync(string teamId)
        {
            var matchedDocuments = await _collection.Find(c => c.Statistics.Any(s => s.Team == teamId)).ToListAsync();

            return matchedDocuments;
        }

        public async Task<IEnumerable<Player>> GetPlayersByListOfIdsAsync(IEnumerable<string> ids)
        {
            var filter = Builders<Player>.Filter.In(l => l.Id, ids);
            var matchedDocuments = await _collection.Find(filter).ToListAsync();

            return matchedDocuments;
        }

        public async Task<IEnumerable<Player>> SearchByNameAsync(string search)
        {
            if (search.First() == '*' || search.Last() == '*')
            {
                search = search.Replace("*", string.Empty);
                var builder = Builders<Player>.Filter;
                var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
                var filter = builder.Regex("Firstname", queryExpr) | builder.Regex("Lastname", queryExpr);
                return await _collection.Find(filter).ToListAsync();
            }

            //var nameFilter = Builders<Player>.Filter.Eq("Firstname", search);
            var nameFilter = Builders<Player>.Filter.Text(search);

            return await _collection.Find(nameFilter).ToListAsync();
        }
        private void BuildDateOfBirthFilterNew(ref FilterDefinition<Player> filter, DateTime? from, DateTime? to) // TODO parse find model => move to helper
        {
            if (from != null)
            {
                filter &= Builders<Player>.Filter.Gte(p => p.Birth.Date, from);
            }

            if (to != null)
            {
                filter &= Builders<Player>.Filter.Lte(p => p.Birth.Date, to);
            }
        }

        public FilterDefinition<Player> BuildFilter(PlayerSearchByParametersModel searchModel) // TODO parse find model => move to helper
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Empty;

            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            if (searchModel.AgeFrom > 0)
            {
                dateTo = DateTime.Now.AddYears(-((int)searchModel.AgeFrom));
            }

            if (searchModel.AgeTo > 0)
            {
                var dateToYears = DateTime.Now.AddYears(-((int)searchModel.AgeTo + 1));

                dateFrom = dateToYears.AddDays(1);
            }

            BuildDateOfBirthFilterNew(ref filter, dateFrom, dateTo); //TODO to calls in a row?

#pragma warning disable CS8604 // Possible null reference argument.
            BuildDateOfBirthFilterNew(ref filter, searchModel.DateOfBirthFrom, searchModel.DateOfBirthTo);
#pragma warning restore CS8604 // Possible null reference argument.
                        
            if (searchModel.CardsFrom > 0)
            {
                //filter &= Builders<Player>.Filter.ElemMatch("Statistics",
                //    Builders<Statistic>.Filter.Gte("Cards.Yellow", searchModel.cardsFrom) | 
                //          Builders<Statistic>.Filter.Gte("Cards.Yellowred", searchModel.cardsFrom) |
                //          Builders<Statistic>.Filter.Gte("Cards.Red", searchModel.cardsFrom));

                filter &= Builders<Player>.Filter.ElemMatch("Statistics",
                    Builders<Statistic>.Filter.Gte("Cards.Yellow", searchModel.CardsFrom));
            }

            if (searchModel.CardsTo > 0)
            {
                //filter &= Builders<Player>.Filter.ElemMatch("Statistics",
                //    Builders<Statistic>.Filter.Lte("Cards.Yellow", searchModel.cardsTo) |
                //          Builders<Statistic>.Filter.Lte("Cards.Yellowred", searchModel.cardsTo) |
                //          Builders<Statistic>.Filter.Lte("Cards.Red", searchModel.cardsTo));

                filter &= Builders<Player>.Filter.ElemMatch("Statistics",
                    Builders<Statistic>.Filter.Lte("Cards.Yellow", searchModel.CardsTo));
            }

            if (searchModel.CardsFrom > 0 && searchModel.CardsTo > 0)
            {

            }

            var filterDebug1 = filter.Render(_collection.DocumentSerializer, _collection.Settings.SerializerRegistry).ToJson();

            var registrySerializer = BsonSerializer.SerializerRegistry;
            var documentSerializer = registrySerializer.GetSerializer<Player>();
            var rendered = filter.Render(documentSerializer, registrySerializer);
            var filterDebug2 = rendered.ToJson();

            return filter;
        }
    }
}
