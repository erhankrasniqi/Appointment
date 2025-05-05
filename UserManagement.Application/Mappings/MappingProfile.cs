using AuthService.Application.DTOs;
using AutoMapper; 
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();  
        }
    }
}