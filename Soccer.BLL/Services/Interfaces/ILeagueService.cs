namespace Soccer.BLL.Services.Interfaces
{
    public interface ILeagueService
    {
        Task CreateAsync(League newLeague);
        
        Task CreateManyAsync(IEnumerable<League> leagues);

        Task UpdateAsync(League updatedLeague);
        
        Task RemoveAsync(string id);
     
    }
}
