using Application.Features.Auth.DTOs;
using Application.Features.Menu.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ShouldMapMethod = mi => false;
            ShouldMapProperty = pi => true;
            // Mission Maps

            CreateMap<User, UserListDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<MenuItem, MenuItemDto>().ReverseMap();

            // Customer Maps
            //CreateMap<Customer, CustomerDto>(); // اگر داری
            // Employee Maps
            //CreateMap<Employee, EmployeeDto>(); // اگر داری
        }
    }
}
