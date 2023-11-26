using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.Shared.Common;

namespace HappyWarehouse.BusinessLogic.Services.IServices
{
    public interface IItemService
    {
        Task<ItemDto> GetItem(int id);
        Task<List<ItemDto>> GetItems();
        Task<PaginatedList<ItemDto>> GetItemsPaged(ItemQueryOption queryOption);
        Task<QueryResult<bool>> AddItem(ItemDto ItemDto);
        Task<QueryResult<bool>> UpdateItem(ItemDto ItemDto);
        Task<QueryResult<bool>> DeleteItem(int id);
    }
}
