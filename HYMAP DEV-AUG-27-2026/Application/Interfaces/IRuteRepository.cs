using Hymap.Domain.Entities;

namespace Hymap.Application.Interfaces
{
    public interface IRuteRepository
    {
        Task<List<Rute>> GetAllAsync(bool? isActive = null);
        Task<Rute?> GetByIdAsync(int id);
        Task<Rute> AddAsync(Rute rute);
        Task UpdateAsync(Rute rute);
        Task DeleteAsync(Rute rute);
        Task<int> GetCountAsync();
    }
}
