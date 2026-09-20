using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class UserProviderRepository : IUserProviderRepository
{
    private readonly ShopDbContext _context;

    public UserProviderRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserProvider entity)
    {
        await _context.UsersProviders.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, int providerId)
    {
        return await _context.UsersProviders
            .AnyAsync(x => x.UserId == userId && x.ProviderId == providerId);
    }

    public async Task<Provider?> GetByNameAsync(string name)
    {
        return await _context.Providers.FirstOrDefaultAsync(x => x.Name == name);
    }
}