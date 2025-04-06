using AuthService.Domain.Entities; 

namespace AuthService.Domain.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
