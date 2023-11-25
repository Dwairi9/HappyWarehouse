using Blazored.LocalStorage;
using Blazored.Toast;
using HappyWarehouse.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

try
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);
	builder.RootComponents.Add<App>("#app");
	builder.RootComponents.Add<HeadOutlet>("head::after");

	builder.Services.AddSingleton(services => (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());
    builder.Services.AddBlazoredLocalStorage();
    builder.Services.AddBlazoredToast();
    builder.Services.AddOptions();
    builder.Services.AddAuthorizationCore();
    builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
    builder.Services.AddScoped(sp => new HttpClient
	{
		BaseAddress = new Uri("https://localhost:7089")
	});

	await builder.Build().RunAsync();
}
catch (Exception ex)
{

	throw;
}
