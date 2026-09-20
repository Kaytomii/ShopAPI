using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Application.Mapping;
using Shop.Application.Queries.Product;
using Shop.Application.Services;
using Shop.Infrastructure.Configuration;
using Shop.Infrastructure.Configuration;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Helpers;
using Shop.Infrastructure.Repositories;
using Shop.Infrastructure.Services;
using ShopApi.Interfaces;
using ShopApi.MiddleWares;
using ShopApi.Services;
using System.Text;

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

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

            builder.Services.Configure<RabbitMqSettings>(
                builder.Configuration.GetSection("RabbitMq")
            );

            builder.Services.AddAutoMapper(_ => { }, typeof(CategoryProfile).Assembly);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });

                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins("https://example.com", "https://www.example.com")
                          .WithMethods("GET", "POST", "PUT", "DELETE")
                          .WithHeaders("Content-Type", "Authorization");
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shop API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });
            });

            builder.Services.AddScoped<Shop.Application.Interfaces.Services.IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, Shop.Infrastructure.Services.AuthService>();
            builder.Services.AddScoped<Shop.Application.Interfaces.Services.ICachingService, RedisCachingService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.AddSingleton<IHashHelper, HashHelper>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddSingleton<IQueueService, RabbitMqService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            builder.Services.AddScoped<IUserProviderRepository, UserProviderRepository>();
            builder.Services.AddScoped<UserAddressService>();

            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));
            builder.Services.AddSingleton<MongoDbService>();
            builder.Services.AddScoped<ProductFeedbackService>();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetProductByIdHandler).Assembly);
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestTimerMiddleware>();
            app.UseMiddleware<CancellationTokenHandleMiddleWare>();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
