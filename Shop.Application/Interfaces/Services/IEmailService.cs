using System;
using System.Collections.Generic;
using System.Text;
using ShopDomain.Models;

namespace Shop.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    Task SendAdminCreatedEmailAsync(string toEmail);
    Task SendOrderCreatedEmailAsync(Guid userId, List<OrderDetail> items, decimal total);
    Task SendOrderWaitingEmailAsync(Guid userId);
}