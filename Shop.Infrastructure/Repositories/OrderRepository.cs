using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _context;

    public OrderRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task CreateOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
}