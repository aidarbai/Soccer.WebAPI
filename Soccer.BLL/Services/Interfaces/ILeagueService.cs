using Soccer.BLL.DTOs;
using Soccer.DAL.Models;

namespace Soccer.BLL.Services.Interfaces
{
    public interface ILeagueService
    {
        Task<IEnumerable<League>> GetAllAsync();

        Task<League?> GetByIdAsync(string id);

        Task<IEnumerable<League>> SearchByListOfLeagueNamesAsync(IEnumerable<string> leagueNames);

        Task CreateAsync(League newLeague);
        
        Task CreateManyAsync(IEnumerable<League> leagues);

        Task UpdateAsync(League updatedLeague);
        
        Task RemoveAsync(string id);

        Task<Dictionary<string, string>> GenerateLeaguesDictionaryAsync(IEnumerable<LeagueDTO> leagues);
    }
}
