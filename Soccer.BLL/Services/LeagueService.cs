using Soccer.DAL.Repositories.Interfaces;

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

        public async Task CreateAsync(League newLeague) => await _repository.CreateAsync(newLeague);

        public async Task CreateManyAsync(IEnumerable<League> leagues) => await _repository.CreateManyAsync(leagues);

        public async Task UpdateAsync(League updatedLeague) => await _repository.UpdateAsync( updatedLeague);

        public async Task RemoveAsync(string id) => await _repository.RemoveAsync(id);
    }
}
