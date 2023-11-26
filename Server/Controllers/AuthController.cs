using HappyWarehouse.BusinessLogic.Services.IServices;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HappyWarehouse.Server.Controllers
{
	[AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;

        public AuthController(UserManager<ApplicationUser> userManager, IUserService userService, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.email);
            if (user != null) 
            {
                var isAuthenticated = await _userManager.CheckPasswordAsync(user, loginModel.password);
                if (isAuthenticated)
                {
                    if (user.Active)
                    {
                        var role = await _roleManager.FindByIdAsync(user.RoleId.ToString());
                        return Ok(new LoginResult 
                        {
                            message = "Login successful.", 
                            jwtBearer = CreateJWT(user, role.Name), 
                            email = loginModel.email, 
                            role = role.Name,
                            success = true }); 
                    }
                    else
                    {
                        return Ok(new LoginResult { message = "You account is deactivated, Please contact support for help", success = false });
                    }
                }
            }
                
            return Ok(new LoginResult { message = "Invalid Username/Password", success = false });
        }

        private string CreateJWT(ApplicationUser user, string roleName)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("524C1F22-6115-4E16-9B6A-3FBF185308F2"));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
			{
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Role", roleName)
			};

            var token = new JwtSecurityToken(issuer: "HappyWarhouseClient", claims: claims, expires: DateTime.Now.AddMinutes(600), signingCredentials: credentials); // NOTE: ENTER DOMAIN HERE
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
