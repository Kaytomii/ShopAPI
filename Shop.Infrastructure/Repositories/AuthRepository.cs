using Shop.Infrastructure.Data;
using ShopDomain.Models;
using Shop.Application.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class AuthRepository(ShopDbContext _context) : IAuthRepository
{
    public async Task<bool> IsExistEmailAsync(string email)
    {
        var userFromDb = await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        return userFromDb != null;
    }

    public async Task<User?> RegisterUserAsync(User user, string hash)
    {
        user.PasswordHash = hash;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return await _context.Users
            .FirstOrDefaultAsync(us => us.Email == user.Email && us.PasswordHash == user.PasswordHash);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}