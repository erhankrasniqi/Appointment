using AuthService.Domain.Entities; 
namespace AuthService.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> UserExistsAsync(string email);
        Task<List<User>> GetAllAsync();
    }
}
