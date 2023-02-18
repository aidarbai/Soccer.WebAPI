namespace Soccer.BLL.Services.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<string>> GetTeamIdsAsync();

        Task CreateAsync(Team newTeam);

        Task CreateManyAsync(IEnumerable<Team> newTeams);

        Task UpdateAsync(Team updatedTeam);

        Task RemoveAsync(string id);
    }
}