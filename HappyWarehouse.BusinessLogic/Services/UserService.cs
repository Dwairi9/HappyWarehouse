using AutoMapper;
using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Repositories.IRepsitories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HappyWarehouse.BusinessLogic.Services.IServices;
using HappyWarehouse.Shared.Common;

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

        public async Task<PaginatedList<UserDto>> GetUsersPaged(QueryOption queryOption)
        {
            var roles = await GetRoles();
            var query = _unitOfWork.Repository<ApplicationUser>().Entities
                .Where(i => (string.IsNullOrEmpty(queryOption.Name) || i.FullName == queryOption.Name));

            var users = await PaginatedList<ApplicationUser>.CreateAsync(query, queryOption.Page, queryOption.Size);
            var usersDto = _mapper.Map<PaginatedList<ApplicationUser>, PaginatedList<UserDto>>(users);

            foreach (var user in usersDto.Items)
            {
                user.RoleName = roles.FirstOrDefault(r => r.Id == user.RoleId).Name;
            }

            return usersDto;
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            var roles = await _unitOfWork.Repository<ApplicationRole>().Entities.ToListAsync();

            return _mapper.Map<List<ApplicationRole>, List<RoleDto>>(roles);
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

            var user = new ApplicationUser { UserName = userDto.Email, Email = userDto.Email, FullName = userDto.FullName, RoleId = userDto.RoleId};

            await _userManager.CreateAsync(user, "P@ssw0rd");
            user = await _userManager.FindByNameAsync(userDto.Email);

            var role = await _roleManager.FindByIdAsync(userDto.RoleId.ToString());
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
            user.RoleId = userDto.RoleId;
            await _unitOfWork.Repository<ApplicationUser>().UpdateAsync(user);

            var role = await _roleManager.FindByIdAsync(userDto.RoleId.ToString());
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
            else if (user.RoleId == 1) 
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "Admin User could not be deleted.",
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
