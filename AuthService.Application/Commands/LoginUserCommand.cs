using AuthService.Domain.Common;
using MediatR;

namespace AuthService.Application.Commands
{
    public class LoginUserCommand : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
