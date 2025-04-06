using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Commands
{
    public class RegisterUserCommand : IRequest<Result>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterUserCommand(string fullName, string email, string password)
        {
            FullName = fullName;
            Email = email;
            Password = password;
        }
    }
}
