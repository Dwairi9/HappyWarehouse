using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.DTOs;
using X.PagedList;

namespace HappyWarehouse.BusinessLogic.Services.IServices
{
    internal interface IUserService
    {
        Task<UserDto> GetUser(int id);
        Task<List<UserDto>> GetUsers();
        Task<IPagedList<UserDto>> GetUsersPaged(QueryOption queryOption);
        Task<QueryResult<bool>> AddUser(UserDto UserDto);
        Task<QueryResult<bool>> UpdateUser(UserDto UserDto);
        Task<QueryResult<bool>> DeleteUser(int id);
        Task<QueryResult<bool>> ChangePassword(int userId, string password);
    }
}
