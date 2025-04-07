

using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user); 
        Task<bool> ExistsByAuthIdAsync(int AuthId);
        Task<List<User>> GetAllAsync();
    }
}
