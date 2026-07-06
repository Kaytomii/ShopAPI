using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Repositories;
using ShopApi.Interfaces;
using ShopApi.MiddleWares;
using ShopApi.Services;

namespace ShopApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
            });
            // Add services to the container.
            builder.Services.AddScoped<Shop.Application.Interfaces.Services.ICategoryService, Shop.Application.Services.CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();

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
