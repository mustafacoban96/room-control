using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto;
using api.Dto.User;
using api.Models;
using AutoMapper;

namespace api.Helper
{
    public class AutoMapperHandler : Profile
    {

        public AutoMapperHandler(){
            CreateMap<User, RegisterDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => new RoleDto
            {
                Name = ur.Role.Name
            })))
            .ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
        }
        
    }
}