using Soccer.BLL.DTOs;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.Services.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllAsync();

        Task<IEnumerable<string>> GetTeamIdsAsync();

        Task<PaginatedResponse<TeamVm>> GetTeamsPaginateAsync(SortAndPageTeamModel model);

        Task<Team> GetByIdAsync(string id);

        Task CreateAsync(Team newTeam);

        Task CreateManyAsync(IEnumerable<Team> newTeams);
        Task UpdateAsync(Team updatedTeam);

        Task RemoveAsync(string id);
        Task<IEnumerable<Team>> SearchByNameAsync(string search);
        
    }
}