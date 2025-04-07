using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Queris
{
    public class GetAllUsersQuery : IRequest<List<UserDto>> { }
}
