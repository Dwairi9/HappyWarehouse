using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace HappyWarehouse.Server.Middlewares
{
	public class CustomAuthorizationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<CustomAuthorizationMiddleware> logger;

		public CustomAuthorizationMiddleware(RequestDelegate next, ILogger<CustomAuthorizationMiddleware> logger)
		{
			_next = next;
			this.logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			var result = true;
			var endpoint = context.GetEndpoint();
			var isAllowAnonymous = endpoint?.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));

			if (isAllowAnonymous == true || !context.Request.Path.StartsWithSegments("/api"))
			{
				await _next(context);
			}
			else
			{
				string token = string.Empty;

				if (!context.Request.Headers.ContainsKey("Authorization"))
				{
					result = false;
				}

				if (result)
				{
					token = context.Request.Headers.First(x => x.Key == "Authorization").Value.ToString().Replace("Bearer ", "");
					token = JsonConvert.DeserializeObject<string>(token);

					try
					{
						var tokenHandler = new JwtSecurityTokenHandler();
						var claimPrinciple = tokenHandler.ValidateToken(token,
							new TokenValidationParameters
							{
								ValidateAudience = false,
								ValidateIssuer = true,
								ValidateLifetime = true,
								ValidateIssuerSigningKey = true,
								ValidIssuer = "HappyWarhouseClient",
								IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("524C1F22-6115-4E16-9B6A-3FBF185308F2")),
								ClockSkew = TimeSpan.Zero
							}, out SecurityToken validateToken);

						if (claimPrinciple == null || claimPrinciple.Claims == null || claimPrinciple.Claims.Count() == 0)
						{
							result = false;
						}
						else
						{
							var appIdentity = new ClaimsIdentity(claimPrinciple.Claims);
							context.User.AddIdentity(appIdentity);
						}
					}
					catch
					{
						result = false;
					}
				}

				if (result) await _next(context);
				else
				{
					context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
					context.Response.ContentType = "text/plain";

					await context.Response.WriteAsync(HttpStatusCode.Unauthorized.ToString());
				}
			}
		}
	}
}
