using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IUserAddressRepository
{
    Task AddAsync(UserAddress address, CancellationToken token);
    Task<IEnumerable<UserAddress>> GetByUserIdAsync(Guid userId, CancellationToken token);
}