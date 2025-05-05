using AuthService.Domain.Common;
using AuthService.Domain.Entities;
using AuthService.Domain.Repositories;
using AuthService.Domain.Security;
using MediatR; 
using Messaging.RabitMQ.Interfaces;
using SharedKernel.Contracts;


namespace AuthService.Application.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRegisteredPublisher _userRegisteredPublisher;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUserRegisteredPublisher userRegisteredPublisher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userRegisteredPublisher = userRegisteredPublisher;
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
             
            var userRegisteredEvent = new UserRegisteredEvent
            {
                UserId = user.Id,
                Email = user.Email
            };

            await _userRegisteredPublisher.PublishUserRegisteredEventAsync(userRegisteredEvent);

            return Result.Success("User registered successfully");
        }
    }
}
