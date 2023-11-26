using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.Shared.Common;

namespace HappyWarehouse.BusinessLogic.Services.IServices
{
    public interface IUserService
    {
        Task<UserDto> GetUser(int id);
        Task<List<UserDto>> GetUsers();
        Task<PaginatedList<UserDto>> GetUsersPaged(QueryOption queryOption);
        Task<List<RoleDto>> GetRoles();
        Task<QueryResult<bool>> AddUser(UserDto UserDto);
        Task<QueryResult<bool>> UpdateUser(UserDto UserDto);
        Task<QueryResult<bool>> DeleteUser(int id);
        Task<QueryResult<bool>> ChangePassword(int userId, string password);
    }
}
