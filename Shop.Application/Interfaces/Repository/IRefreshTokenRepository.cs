using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetAsync(string token);
    Task AddAsync(RefreshToken token);
    Task RemoveAsync(RefreshToken token);
}
