using AuthService.Domain.Common;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.Security;
using MediatR;


namespace AuthService.Application.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsAsync(request.Email))
            {
                return Result.Failure("User already exists");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var user = new User(request.FullName, request.Email, passwordHash);
            await _userRepository.AddAsync(user);

            return Result.Success("User registered successfully");
        }
    }
}
