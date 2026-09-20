using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IUserProviderRepository
{
    Task AddAsync(UserProvider entity);
    Task<bool> ExistsAsync(Guid userId, int providerId);
    Task<Provider?> GetByNameAsync(string name);
}