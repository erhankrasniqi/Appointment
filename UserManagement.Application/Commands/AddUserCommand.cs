
using MediatR;
using UserManagement.Application.DTOs;
using UserManagement.Domain.Common;

namespace UserManagement.Application.Commands
{
    public class AddUserCommand : IRequest<Result>
    {
        public string Name { get;  set; }
        public string SurnameName { get;  set; }
        public DateTime? BrithDate { get;  set; }
        public int AuthId { get; set; }  
        public AddressDto Address { get; init; }

    }
}
