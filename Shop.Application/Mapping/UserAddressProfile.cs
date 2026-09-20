using AutoMapper;
using Shop.Application.DTOs.UserDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Mapping;

public class UserAddressProfile : Profile
{
    public UserAddressProfile()
    {
        CreateMap<UserAddressCreateDto, UserAddress>();
    }
}