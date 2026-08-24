using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string toEmail, string resetLink);
    Task SendAdminCreatedEmailAsync(string toEmail);
}