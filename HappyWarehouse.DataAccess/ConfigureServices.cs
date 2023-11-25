using HappyWarehouse.DataAccess.DataContext;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HappyWarehouse.DataAccess.Seeds;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using HappyWarehouse.DataAccess.Repositories.IRepsitories;
using HappyWarehouse.DataAccess.Repositories;

namespace HappyWarehouse.DataAccess
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString)) 
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            connectionString = connectionString.Replace("{AppDir}", path);

            services.AddDbContext<HappyWarehouseDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<SeedHelper>();
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddRoleManager<RoleManager<ApplicationRole>>()
                    .AddEntityFrameworkStores<HappyWarehouseDbContext>()
                    .AddDefaultTokenProviders();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            //services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
