using AutoMapper;
using Soccer.BLL.DTOs;
using Soccer.BLL.Services.Interfaces;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.Services
{
    public class TeamService : ITeamService
    {

        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;
        public TeamService(
            ITeamRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Team>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<IEnumerable<string>> GetTeamIdsAsync()
        {
            var teams = await _repository.GetAllAsync();
            return teams.Select(t => t.Id);
        }
        public async Task<PaginatedResponse<TeamVm>> GetTeamsPaginateAsync(SortAndPageTeamModel model)
        {
            var teams = await _repository.GetTeamsPaginatedAsync(model);

            return teams;
        }

        public async Task<Team> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);

        public async Task CreateAsync(Team newTeam) => await _repository.CreateAsync(newTeam);

        public async Task CreateManyAsync(IEnumerable<Team> newTeams) => await _repository.CreateManyAsync(newTeams);

        public async Task UpdateAsync(Team updatedTeam) => await _repository.UpdateAsync(updatedTeam);

        public async Task RemoveAsync(string id) => await _repository.RemoveAsync(id);

        public async Task<IEnumerable<Team>> SearchByNameAsync(string search) => await _repository.SearchByNameAsync(search);

    }
}