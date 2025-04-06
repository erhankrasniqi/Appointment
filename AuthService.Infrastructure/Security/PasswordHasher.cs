using Microsoft.AspNetCore.Identity;
using AuthService.Domain.Security; 
using AuthService.Domain.Entities;

namespace AuthService.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string password, string storedPasswordHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(null, storedPasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }

}
