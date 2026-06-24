using ShopDomain.Models;
using System.Text.Json;

namespace ShopApp.MiddleWares;

public class UserCheckMiddleware
{
    private readonly RequestDelegate _next;

    public UserCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == "POST" &&
            context.Request.Path == "/api/user/register")
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            string body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0;

            var json = JsonDocument.Parse(body).RootElement;

            int id = json.GetProperty("id").GetInt32();
            string login = json.GetProperty("Login").GetString();

            if (id != 1 || login != "admin")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "No authorization"
                });
                return;
            }
        }

        await _next(context);
    }
}
