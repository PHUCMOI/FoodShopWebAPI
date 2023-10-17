using DataAccessLayer.DataAccess;
using DataAccessLayer.DataAccessInterfaces;
using Fooding_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services_Layer.Service;
using Services_Layer.ServiceInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services_Layer
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIService(this IServiceCollection services, IConfiguration configuration)
        {
            // Service
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ICheckImageService, CheckImageService>();
            services.AddScoped<ITokenService, TokenService>();

            // Data Access
            services.AddScoped<IProductDAO, ProductDAO>();
            services.AddScoped<ICategoryDAO, CategoryDAO>();
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IOrderDAO, OrderDAO>();
            services.AddScoped<IOrderDetailDAO, OrderDetailDAO>();

            services.AddScoped<IAutoMapperService, AutoMapperService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
            });
            return services;
        }
    }
}
