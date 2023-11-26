using AutoMapper;
using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.BusinessLogic.Services.IServices;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Repositories.IRepsitories;
using Microsoft.EntityFrameworkCore;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.Shared.Common;

namespace HappyWarehouse.BusinessLogic.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehouseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<WarehouseDto> GetWarehouse(int id) 
        {
            var warehouse = await _unitOfWork.Repository<Warehouse>().Entities.FirstOrDefaultAsync(w => w.Id == id);

            return _mapper.Map<Warehouse, WarehouseDto>(warehouse);
        }

        public async Task<List<WarehouseDto>> GetWarehouses()
        {
            var warehouses = await _unitOfWork.Repository<Warehouse>().Entities.Include(w => w.Country).ToListAsync();

            return _mapper.Map<List<Warehouse>, List<WarehouseDto>>(warehouses);
        }

        public async Task<PaginatedList<WarehouseDto>> GetWarehousesPaged(QueryOption queryOption)
        {
            var query = _unitOfWork.Repository<Warehouse>().Entities.Include(w => w.Country).Include(w => w.Items)
                .Where(i => (string.IsNullOrEmpty(queryOption.Name) || i.Name == queryOption.Name));

            var warehouses = await PaginatedList<Warehouse>.CreateAsync(query, queryOption.Page, queryOption.Size);

            return _mapper.Map<PaginatedList<Warehouse>, PaginatedList<WarehouseDto>>(warehouses);
        }

        public async Task<QueryResult<bool>> AddWarehouse(WarehouseDto warehouseDto)
        {
            try
            {
                var existWarehouse = await _unitOfWork.Repository<Warehouse>().Entities.FirstOrDefaultAsync(w => w.Name == warehouseDto.Name);
                if (existWarehouse != null)
                {
                    return new QueryResult<bool>()
                    {
                        Success = false,
                        Message = "Warehouse already exist.",
                    };
                }

                var warehouse = _mapper.Map<WarehouseDto, Warehouse>(warehouseDto);
                var addResult = await _unitOfWork.Repository<Warehouse>().AddAsync(warehouse);

                return new QueryResult<bool>()
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<QueryResult<bool>> UpdateWarehouse(WarehouseDto warehouseDto)
        {
            var existWarehouse = await _unitOfWork.Repository<Warehouse>().Entities.FirstOrDefaultAsync(w => w.Id != warehouseDto.Id && w.Name == warehouseDto.Name);
            if (existWarehouse != null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "Warehouse already exist.",
                };
            }

            var warehouse = _mapper.Map<WarehouseDto, Warehouse>(warehouseDto);
            await _unitOfWork.Repository<Warehouse>().UpdateAsync(warehouse);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }

        public async Task<QueryResult<bool>> DeleteWarehouse(int id)
        {
            var warehouse = await _unitOfWork.Repository<Warehouse>().Entities.FirstOrDefaultAsync(w => w.Id == id);
            if (warehouse == null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "Warehouse NOT found.",
                };
            }
            
            await _unitOfWork.Repository<Warehouse>().DeleteAsync(warehouse);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }
    }
}
