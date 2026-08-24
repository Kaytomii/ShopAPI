using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Enums;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class AdminService : IAdminService
{
    private readonly IAuthRepository _authRepository;
    private readonly IHashHelper _hashHelper;

    public AdminService(IAuthRepository authRepository, IHashHelper hashHelper)
    {
        _authRepository = authRepository;
        _hashHelper = hashHelper;
    }

    public async Task<User> CreateAdminAsync(string email, string password)
    {
        var hash = _hashHelper.Hash(password);

        var user = new User
        {
            Email = email,
            PasswordHash = hash,
            Role = UserRole.Admin
        };

        await _authRepository.RegisterUserAsync(user, hash);
        return user;
    }

    public async Task<User> CreateModeratorAsync(string email, string password)
    {
        var hash = _hashHelper.Hash(password);

        var user = new User
        {
            Email = email,
            PasswordHash = hash,
            Role = UserRole.Moderator
        };

        await _authRepository.RegisterUserAsync(user, hash);
        return user;
    }

    public async Task<bool> ResetUserPasswordAsync(Guid userId, string newPassword)
    {
        var user = await _authRepository.GetUserByIdAsync(userId);
        if (user == null)
            return false;

        var hash = _hashHelper.Hash(newPassword);
        await _authRepository.UpdatePasswordAsync(userId, hash);

        return true;
    }
}