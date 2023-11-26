using Blazored.LocalStorage;
using Blazored.Toast;
using HappyWarehouse.Client;
using HappyWarehouse.Client.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using HappyWarehouse.Client.Services;

try
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);
	builder.RootComponents.Add<App>("#app");
	builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services
    .AddApiAuthorization()
    .AddAccountClaimsPrincipalFactory<CustomAccountClaimsPrincipalFactory>();

    builder.Services.AddSingleton(services => (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());
    builder.Services.AddBlazoredLocalStorage();
    builder.Services.AddBlazoredToast();
    builder.Services.AddBlazorBootstrap();
    builder.Services.AddOptions();
    builder.Services.AddAuthorizationCore();
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
    builder.Services.AddHttpClient<ApiClientService>();
    
    await builder.Build().RunAsync();
}
catch (Exception ex)
{

	throw;
}
