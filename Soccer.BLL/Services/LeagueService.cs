using AutoMapper;
using MongoDB.Driver;
using Soccer.BLL.DTOs;
using Soccer.BLL.Services.Interfaces;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;
using System;

namespace Soccer.BLL.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _repository;
        private readonly IMapper _mapper;

        public LeagueService(
            ILeagueRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<League>> GetAllAsync() => await _repository.GetAllAsync(); // TODO move models from DAL to COMMON

        public async Task<League?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<League>> SearchByListOfLeagueNamesAsync(IEnumerable<string> leagueNames) => await _repository.SearchByListOfLeagueNamesAsync(leagueNames);

        public async Task CreateAsync(League newLeague) => await _repository.CreateAsync(newLeague);
        public async Task CreateManyAsync(IEnumerable<League> leagues) => await _repository.CreateManyAsync(leagues);

        public async Task UpdateAsync(League updatedLeague) => await _repository.UpdateAsync( updatedLeague);

        public async Task RemoveAsync(string id) => await _repository.RemoveAsync(id);

        public async Task<Dictionary<string, string>> GenerateLeaguesDictionaryAsync(IEnumerable<LeagueDTO> leagues)
        {
            var leaguesDictionary = leagues.ToDictionary(keySelector: l => l.Name, elementSelector: l => l.Id.ToString());

            var leaguesFromDb = await SearchByListOfLeagueNamesAsync(leaguesDictionary.Keys);

            foreach (var item in leaguesFromDb)
            {
                var idFromDb = item.Id;
                var idInDictionary = leaguesDictionary[item.Name];
                if (!string.Equals(idFromDb, idInDictionary, StringComparison.OrdinalIgnoreCase))
                {
                    leaguesDictionary[item.Name] = idFromDb;
                }
            }

            leaguesDictionary = await CreateMissingLeagues(leagues, leaguesFromDb, leaguesDictionary);

            return leaguesDictionary;
        }

        private async Task<Dictionary<string, string>> CreateMissingLeagues(IEnumerable<LeagueDTO> leagues, IEnumerable<League> leaguesFromDb, Dictionary<string, string> leaguesDictionary)
        {
            var missingLeagueDTOs = leagues.ExceptBy(leaguesFromDb.Select(l => l.Name), x => x.Name);

            if (missingLeagueDTOs.Any())
            {
                var missingLeagues = _mapper.Map<IEnumerable<League>>(missingLeagueDTOs);

                foreach (var item in missingLeagues)
                {
                    if (item.Id == "0" || string.IsNullOrEmpty(item.Id))
                    {
                        var randomId = Guid.NewGuid().ToString();
                        item.Id = randomId;
                        leaguesDictionary[item.Name] = randomId;
                    }
                }

                await CreateManyAsync(missingLeagues);
            }

            return leaguesDictionary;
        }
    }
}
