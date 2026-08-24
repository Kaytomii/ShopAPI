using Shop.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Shop.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _smtpHost = "smtp.gmail.com";
    private readonly int _smtpPort = 587;

    private readonly string _emailFrom = "yourmail@gmail.com";
    private readonly string _emailPassword = "your_app_password";

    public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
    {
        var message = new MailMessage(_emailFrom, toEmail)
        {
            Subject = "Password Reset",
            Body = $"Click the link to reset your password:\n{resetLink}",
            IsBodyHtml = false
        };

        using var client = new SmtpClient(_smtpHost, _smtpPort)
        {
            Credentials = new NetworkCredential(_emailFrom, _emailPassword),
            EnableSsl = true
        };

        await client.SendMailAsync(message);
    }

    public async Task SendAdminCreatedEmailAsync(string toEmail)
    {
        var message = new MailMessage(_emailFrom, toEmail)
        {
            Subject = "Admin Account Created",
            Body = "Your admin account has been successfully created.",
            IsBodyHtml = false
        };

        using var client = new SmtpClient(_smtpHost, _smtpPort)
        {
            Credentials = new NetworkCredential(_emailFrom, _emailPassword),
            EnableSsl = true
        };

        await client.SendMailAsync(message);
    }
}
