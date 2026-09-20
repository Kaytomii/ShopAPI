using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ShopDbContext _context;

    public RefreshTokenRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task AddAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(RefreshToken token)
    {
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
    }
}