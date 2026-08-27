using Hymap.Domain.Entities;

namespace Hymap.Application.Interfaces
{
    public interface IWilayahRepository
    {
        Task<List<Wilayah>> GetAllAsync(bool? isActive = null);
        Task<Wilayah?> GetByIdAsync(int id);
        Task<Wilayah> AddAsync(Wilayah wilayah);
        Task UpdateAsync(Wilayah wilayah);
        Task DeleteAsync(Wilayah wilayah);
        Task<int> GetCountAsync();
    }
}
