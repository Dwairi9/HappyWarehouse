using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.BusinessLogic.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.BusinessLogic.DTOs.Common;

namespace HappyWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _userService.GetRoles());
        }

        [HttpGet("GetUsersPaged")]
        public async Task<IActionResult> GetUsersPaged(int? page = 1)
        {
            return Ok(await _userService.GetUsersPaged(new QueryOption() { Page = (int)page }));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddUser(UserDto warehouseDto)
        {
            return Ok(await _userService.AddUser(warehouseDto));
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var result = new QueryResult<bool> {Success = false };

            try
            {
                var user = await _userManager.FindByIdAsync(changePasswordDto.Id.ToString());
                if (user != null)
                {
                    bool isValidPassword = await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
                    if (isValidPassword)
                    {
                        var changeResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                        if (changeResult.Succeeded)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.Message = string.Join(Environment.NewLine, changeResult.Errors.Select(e=> e.Description));
                        }
                    }
                    else 
                    {
                        result.Message = "Invalid Current password.";
                    }
                }
                else 
                {
                    result.Message = "User NOT found";
                }
            }
            catch (Exception ex)
            {
                result.Message = "Something went wrong please try again later";
            }

            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser(UserDto warehouseDto)
        {
            return Ok(await _userService.UpdateUser(warehouseDto));
        }

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return Ok(await _userService.DeleteUser(id));
        }
    }
}
