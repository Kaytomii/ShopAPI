using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IAuthRepository
{
    Task<User?> RegisterUserAsync(User user, string hash);
    Task<bool> IsEmailExistsAsync(string email);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    Task SaveResetTokenAsync(Guid userId, string token, DateTime expiresAt);
    Task<User?> GetUserByResetTokenAsync(string token);
    Task UpdatePasswordAsync(Guid userId, string newHash);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
}