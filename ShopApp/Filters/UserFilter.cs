using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace ShopApp.Filters
{
    public class UserFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (request.Method == "POST" &&
                request.Path == "/api/user/register")
            {
                request.EnableBuffering();

                using var reader = new StreamReader(request.Body, leaveOpen: true);
                string body = await reader.ReadToEndAsync();

                request.Body.Position = 0;

                var json = JsonDocument.Parse(body).RootElement;

                int id = json.GetProperty("id").GetInt32();
                string login = json.GetProperty("Login").GetString();

                if (id != 1 || login != "admin")
                {
                    context.Result = new JsonResult(new
                    {
                        message = "No authorization"
                    })
                    {
                        StatusCode = 401
                    };
                    return;
                }
            }

            await next();
        }
    }
}
