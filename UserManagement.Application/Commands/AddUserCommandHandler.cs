using MediatR;
using UserManagement.Domain.Common;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Repositories;

namespace UserManagement.Application.Commands
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public AddUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        { 
            bool userExists = await _userRepository.ExistsByAuthIdAsync(request.AuthId);
            if (userExists)
            {
                return Result.Failure("User already exists");
            }
             
            Address? address = null;
            if (request.Address != null)
            {
                address = new Address
                {
                    Street = request.Address.Street,
                    City = request.Address.City,
                    PostalCode = request.Address.PostalCode,
                    Country = request.Address.Country,
                    Phone = request.Address.Phone
                };
            }
             
            var user = new User(
                name: request.Name ?? "Unknown",
                surnameName: request.SurnameName ?? "Unknown",
                brithDate: request.BrithDate ?? new DateTime(2000, 1, 1),
                authId: request.AuthId,
                address: address
            );

            await _userRepository.AddAsync(user);

            return Result.Success("User added successfully");
        }
    }
}
