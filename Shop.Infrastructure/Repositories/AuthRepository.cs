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
    private readonly ShopDbContext _context;

    public async Task<bool> IsEmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> RegisterUserAsync(User user, string hash)
    {
        user.PasswordHash = hash;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task SaveResetTokenAsync(Guid userId, string token, DateTime expiresAt)
    {
        var rt = new ResetToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = expiresAt
        };

        await _context.ResetTokens.AddAsync(rt);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByResetTokenAsync(string token)
    {
        var rt = await _context.ResetTokens
            .FirstOrDefaultAsync(t => t.Token == token && t.ExpiresAt > DateTime.UtcNow);

        if (rt == null)
            return null;

        return await _context.Users.FirstOrDefaultAsync(u => u.Id == rt.UserId);
    }

    public async Task UpdatePasswordAsync(Guid userId, string newHash)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return;

        user.PasswordHash = newHash;
        await _context.SaveChangesAsync();
    }
}