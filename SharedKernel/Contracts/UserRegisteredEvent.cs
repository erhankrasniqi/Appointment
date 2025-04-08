using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Contracts
{
    public class UserRegisteredEvent
    {
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
