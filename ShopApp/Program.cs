
using ShopApp.Interfaces;
using ShopApp.MiddleWares;
using ShopApp.Services;

namespace ShopApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

           
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseMiddleware<UserCheckMiddleware>();
            app.MapControllers();
            

            app.Run();
        }
    }
}
