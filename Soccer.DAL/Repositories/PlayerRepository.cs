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
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly IMapper _mapper;
        private static readonly Dictionary<PlayerSortBy, Expression<Func<Player, object>>> _dictionary = new ()
        {
            { PlayerSortBy.FIRSTNAME, x => x.Firstname},
            { PlayerSortBy.LASTNAME, x => x.Lastname}
        };
        public PlayerRepository(IConfiguration configuration, IMapper mapper) : base(configuration)
        {
            _mapper = mapper;
        }
        
        public async Task<PaginatedResponse<PlayerVM>> GetPlayersPaginatedAsync(SortAndPagePlayerModel model)
        {
            var count = await _collection.EstimatedDocumentCountAsync();

            var players = await _collection
                                .Find(Builders<Player>.Filter.Empty)
                                .Sort(GetSortDefinition(model))
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Limit(model.PageSize)
                                .ToListAsync();

            var result = new PaginatedResponse<PlayerVM>
            {
                ItemsCount = count,
                PageSize = model.PageSize,
                TotalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize)),
                PageNumber = model.PageNumber,

                Results = _mapper.Map<List<PlayerVM>>(players)
            };

            return result;
        }

        public async Task<List<Player>> GetPlayersForPaginatedResponseAsync(SortAndPagePlayerModel model, BsonDocument? query = null)
        {
            query ??= new BsonDocument();

            var players = await _collection
                                .Find(query)
                                .Sort(GetSortDefinition(model))
                                .Skip((model.PageNumber - 1) * model.PageSize)
                                .Limit(model.PageSize)
                                .ToListAsync();

            return players;
        }

        public async Task<long> GetPlayersCountAsync()
        {
            var count = await _collection.EstimatedDocumentCountAsync();
            
            return count;
        }

        public async Task<long> GetPlayersQueryCountAsync(BsonDocument query)
        {
            var count = await _collection.Find(query).CountDocumentsAsync();

            return count;
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

            return await _collection.Find(p => p.Firstname == search || p.Lastname == search).ToListAsync(); //TODO we need an index and change to builder
        }

        public BsonDocument BuildQuery(int from, int to) // TODO parse find model => move to helper
        {
            return new BsonDocument
            {
                {"Age" , new BsonDocument {
                    { "$gte" , from},
                    { "$lte" , to}
                }}
            };

        }
    }
}
