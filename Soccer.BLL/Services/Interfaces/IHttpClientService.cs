using Soccer.DAL.Models;

namespace Soccer.BLL.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<T?> GetDataAsync<T>(string url);
    }
}
