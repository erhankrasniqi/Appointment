

using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Data
{
    public class UserManagmentDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserManagmentDbContext(DbContextOptions<UserManagmentDbContext> options)
            : base(options)
        {
        }
    }
}