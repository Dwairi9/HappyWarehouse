using System.Reflection;
using HappyWarehouse.BusinessLogic.Services.IServices;
using HappyWarehouse.BusinessLogic.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());;
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICountryService, CountryService>();

        return services;
    }
}

