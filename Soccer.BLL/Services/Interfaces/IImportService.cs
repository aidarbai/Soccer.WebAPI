namespace Soccer.BLL.Services.Interfaces
{
    public interface IImportService
    {
        Task ImportLeagueAsync();

        Task ImportTeamsByLeagueAsync();

        Task ImportAllPlayersByTeamsListAsync();
    }
}
