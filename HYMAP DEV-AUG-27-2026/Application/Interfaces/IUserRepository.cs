using Hymap.Domain.Entities;

namespace Hymap.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task InitializeFirstAdminAsync(); 
    }
}
