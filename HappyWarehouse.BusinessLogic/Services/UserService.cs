using AutoMapper;
using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Repositories.IRepsitories;
using X.PagedList;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HappyWarehouse.BusinessLogic.Services.IServices;

namespace HappyWarehouse.BusinessLogic.Services
{
    internal class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().Entities.FirstOrDefaultAsync(w => w.Id == id);

            return _mapper.Map<ApplicationUser, UserDto>(user);
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users =  await _unitOfWork.Repository<ApplicationUser>().Entities.ToListAsync();

            return _mapper.Map<List<ApplicationUser>, List<UserDto>>(users);
        }

        public async Task<IPagedList<UserDto>> GetUsersPaged(QueryOption queryOption)
        {
            var users = await _unitOfWork.Repository<ApplicationUser>().Entities
                .Where(i => (string.IsNullOrEmpty(queryOption.Name) || i.FullName == queryOption.Name)).ToPagedListAsync(queryOption.Page, queryOption.Size);

            return _mapper.Map<IPagedList<ApplicationUser>, IPagedList<UserDto>>(users);
        }

        public async Task<QueryResult<bool>> AddUser(UserDto userDto)
        {
            var existUser = await _unitOfWork.Repository<ApplicationUser>().Entities.FirstOrDefaultAsync(w => w.Email == userDto.Email);
            if (existUser != null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "User already exist.",
                };
            }

            var user = new ApplicationUser { UserName = userDto.Email, Email = userDto.Email, FullName = userDto.FullName};

            await _userManager.CreateAsync(user, "P@ssw0rd");
            user = await _userManager.FindByNameAsync(userDto.Email);

            var role = await _roleManager.FindByIdAsync(userDto.RoleId);
            await _userManager.AddToRoleAsync(user!, role.Name);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }

        public async Task<QueryResult<bool>> UpdateUser(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "User NOT found.",
                };
            }

            user.FullName = userDto.FullName;
            user.Active = userDto.Active;
            await _unitOfWork.Repository<ApplicationUser>().UpdateAsync(user);

            var role = await _roleManager.FindByIdAsync(userDto.RoleId);
            if (!(await _userManager.IsInRoleAsync(user, role.Name))) 
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());
                await _userManager.AddToRoleAsync(user!, role.Name);
            }

            return new QueryResult<bool>()
            {
                Success = true
            };
        }

        public async Task<QueryResult<bool>> DeleteUser(int id)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().Entities.FirstOrDefaultAsync(w => w.Id == id);
            if (user == null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "User NOT found.",
                };
            }

            await _unitOfWork.Repository<ApplicationUser>().DeleteAsync(user);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }

        public async Task<QueryResult<bool>> ChangePassword(int userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "User NOT found.",
                };
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
            await _unitOfWork.Repository<ApplicationUser>().UpdateAsync(user);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }
    }
}
