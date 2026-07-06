using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository(ShopDbContext _context) : ICategoryRepository
{
    public async Task<int?> AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChangesAsync();
        return category.Id;
    }
}
