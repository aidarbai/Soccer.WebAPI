using Soccer.DAL.Models;

namespace Soccer.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : Document
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);

        Task CreateAsync(T newDocument);

        Task CreateManyAsync(IEnumerable<T> newDocuments);
        Task UpdateAsync(T updatedDocument);

        Task RemoveAsync(string id);
    }
}
