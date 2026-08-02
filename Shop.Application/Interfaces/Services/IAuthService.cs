using System;
using System.Collections.Generic;
using System.Text;
using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Interfaces.Services;

public interface IAuthService
{
    Task<(UserReadDTO? User, string? Token)> RegisterAsync(UserCreateDTO dto);
    Task<(UserReadDTO? User, string? Token)> LoginAsync(UserLoginDTO dto);
}
