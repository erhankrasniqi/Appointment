using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class User : BaseEntity
    { 
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        public User(string fullName, string email, string passwordHash)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
