using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class UserAddressRepository : IUserAddressRepository
{
    private readonly ShopDbContext _context;

    public UserAddressRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserAddress address, CancellationToken token)
    {
        await _context.UserAddresses.AddAsync(address, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<UserAddress>> GetByUserIdAsync(Guid userId, CancellationToken token)
    {
        return await _context.UserAddresses
            .Where(x => x.UserId == userId)
            .ToListAsync(token);
    }
}