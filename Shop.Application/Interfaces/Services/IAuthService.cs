using System;
using System.Collections.Generic;
using System.Text;
using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Interfaces.Services;

public interface IAuthService
{
    Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto);
    Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> LoginAsync(UserLoginDTO dto);

}
