using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface IAdminService
{
    Task<User> CreateAdminAsync(string email, string password);
    Task<User> CreateModeratorAsync(string email, string password);
    Task<bool> ResetUserPasswordAsync(Guid userId, string newPassword);
}