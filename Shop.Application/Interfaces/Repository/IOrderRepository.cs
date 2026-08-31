using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IOrderRepository
{
    Task CreateOrderAsync(Order order);
}