using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using X.PagedList;
using HappyWarehouse.Shared.Common;

namespace HappyWarehouse.BusinessLogic.Services.IServices
{
    public interface IWarehouseService
    {
        Task<WarehouseDto> GetWarehouse(int id);
        Task<List<WarehouseDto>> GetWarehouses();
        Task<PaginatedList<WarehouseDto>> GetWarehousesPaged(QueryOption queryOption);
        Task<QueryResult<bool>> AddWarehouse(WarehouseDto warehouseDto);
        Task<QueryResult<bool>> UpdateWarehouse(WarehouseDto warehouseDto);
        Task<QueryResult<bool>> DeleteWarehouse(int id);
    }
}
