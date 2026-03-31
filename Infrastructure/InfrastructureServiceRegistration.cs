using Application.Common;
using Application.Interfaces;
using Application.Interfaces.AuthenticationInterface;
using Application.Interfaces.FastFoodInterface;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Repositories.AuthenticationRepositories;
using Infrastructure.Repositories.SMS;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            //Register Authentication
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            services.AddScoped<IMenuPermissionRepository, MenuPermissionRepository>();
            services.AddMemoryCache();

            services.AddScoped<IS3Storage, S3Storage>();


            services.AddHttpClient(); // یا AddHttpClient(nameof(SmsIrService))
            services.AddScoped<ISmsService, SmsIrService>();
            services.AddScoped<IMessageHistoryService, MessageHistoryService>();


            services.AddScoped<IFoodCategoryService, FoodCategoryService>();
            services.AddScoped<IFoodItemService, FoodItemService>();
            services.AddScoped<IFoodImageService, FoodImageService>();
            services.AddScoped<ISubscriptionCustomerService, SubscriptionCustomerService>();
            services.AddScoped<IMenuPublicService, MenuPublicService>();
            services.AddScoped<IMenuThemeService, MenuThemeService>();


            return services;
        }
    }
}
