using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HappyWarehouse.Server.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

		public AuthController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
        [Route("api/auth/login")]
        public async Task<LoginResult> Post([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.email);
            if (user != null) 
            {
                var isAuthenticated = await _userManager.CheckPasswordAsync(user, loginModel.password);
                if (isAuthenticated) 
                {
					return new LoginResult { message = "Login successful.", jwtBearer = CreateJWT(user), email = loginModel.email, success = true };
				}
            }
                
            return new LoginResult { message = "Invalid Username/Password", success = false };
        }

        private string CreateJWT(ApplicationUser user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("524C1F22-6115-4E16-9B6A-3FBF185308F2"));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
			{
                new Claim(ClaimTypes.Name, user.Email),
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
			};

            var token = new JwtSecurityToken(issuer: "https://localhost:7089/", audience: "https://localhost:7089/", claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials); // NOTE: ENTER DOMAIN HERE
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
