using HappyWarehouse.DataAccess.Seeds;
using HappyWarehouse.DataAccess;
using HappyWarehouse.Server.Services;
using HappyWarehouse.DataAccess.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer; // NOTE: THIS LINE OF CODE IS NEWLY ADDED
using Microsoft.IdentityModel.Tokens;
using HappyWarehouse.Server.Middlewares;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidIssuer = "HappyWarhouseClient",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("524C1F22-6115-4E16-9B6A-3FBF185308F2")) // NOTE: THIS SHOULD BE A SECRET KEY NOT TO BE SHARED; A GUID IS RECOMMENDED. DO NOT REUSE THIS GUID.
        };
    });

	builder.Services.AddCors(policy =>
	{
		policy.AddPolicy("CorsPolicy", opt => opt
			.AllowAnyOrigin()
			.AllowAnyHeader()
			.AllowAnyMethod());
	});

	builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    builder.Services.AddDataAccessServices(builder.Configuration);
    builder.Services.AddBusinessLogicServices();
    builder.Services.AddScoped<ICurrentUser, CurrentUser>();

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<SeedHelper>();
            await initialiser.InitialiseAsync();
            await initialiser.SeedAsync();
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during database initialisation.");

            throw;
        }
    }
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
	app.UseBlazorFrameworkFiles();
	app.UseStaticFiles();
    app.UseAuthentication();
    app.UseRouting();
	app.UseCors("CorsPolicy");
	app.UseAuthorization();
    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");

	app.UseMiddleware<CustomAuthorizationMiddleware>();
	app.Run();
}
catch (Exception ex)
{

    throw;
}