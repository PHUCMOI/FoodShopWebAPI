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
using ServiceLayer.Service;
using ServiceLayer.ServiceInterfaces;
using Microsoft.Extensions.FileProviders;

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
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IAutoMapperService, AutoMapperService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IPaypalService, PaypalService>();
            services.AddScoped<IRestaurantService,  RestaurantService>();
            services.AddScoped<IRestaurantMapService, RestaurantMapService>();
            services.AddScoped<IReservationService, ReservationService>();

            // Data Access
            services.AddScoped<IProductDAO, ProductDAO>();
            services.AddScoped<ICategoryDAO, CategoryDAO>();
            services.AddScoped<IUserDAO, UserDAO>();
            services.AddScoped<IOrderDAO, OrderDAO>();
            services.AddScoped<IOrderDetailDAO, OrderDetailDAO>();
            services.AddScoped<ICartDAO, CartDAO>();
            services.AddScoped<IDashboardDAO, DashboardDAO>();
            services.AddScoped<IRestaurantDAO, RestaurantDAO>();
            services.AddScoped<IRestaurantMapDAO, RestaurantMapDAO>();
            services.AddScoped<IReservationDAO, ReservationDAO>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
            });
            return services;
        }
    }
}
