using HappyWarehouse.DataAccess.DataContext;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HappyWarehouse.DataAccess.Seeds;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace HappyWarehouse.DataAccess
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString)) 
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
            //System.Diagnostics.Debugger.Launch();
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            connectionString = connectionString.Replace("{AppDir}", path);

            services.AddDbContext<HappyWarehouseDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<SeedHelper>();
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddRoleManager<RoleManager<ApplicationRole>>()
                    .AddEntityFrameworkStores<HappyWarehouseDbContext>()
                    .AddDefaultTokenProviders();

            //services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<HappyWarehouseDbContext>();
            //.AddEntityFrameworkStores<HappyWarehouseDbContext>().AddDefaultTokenProviders();
            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<HappyWarehouseDbContext>();
            //.AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

            //services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
