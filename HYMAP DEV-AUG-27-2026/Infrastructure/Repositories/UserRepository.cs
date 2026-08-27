using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;
using Hymap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hymap.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task InitializeFirstAdminAsync()
        {
            await _context.Database.EnsureCreatedAsync(); 
        }
    }
}
