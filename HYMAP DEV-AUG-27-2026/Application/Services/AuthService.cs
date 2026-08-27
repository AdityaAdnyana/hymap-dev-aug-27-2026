using Hymap.Application.Interfaces;
using Hymap.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hymap.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            
            if (user == null) return false;

            // Verifikasi kecocokan password dengan hash yang ada di database
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
