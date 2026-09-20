using System;
using System.Collections.Generic;
using System.Text;
using Shop.Application.DTOs.UserDTOs;
using ShopDomain.Models;

namespace Shop.Application.Interfaces.Services;

public interface IAuthService
{
    Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> RegisterAsync(UserCreateDTO dto);
    Task<(UserReadDTO? User, string? AccessToken, string? RefreshToken)> LoginAsync(UserLoginDTO dto);
    Task<string?> RefreshAsync(string refreshToken);
    Task<User?> GetUserByEmailAsync(string email);
    Task CreateExternalUserAsync(User user);
    Task<string> GenerateRefreshTokenAsync(Guid userId);

}
