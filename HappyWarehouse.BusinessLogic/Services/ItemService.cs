using AutoMapper;
using HappyWarehouse.BusinessLogic.Services.IServices;
using HappyWarehouse.BusinessLogic.DTOs.Common;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.DataAccess.Repositories.IRepsitories;
using Microsoft.EntityFrameworkCore;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.Shared.Common;

namespace HappyWarehouse.BusinessLogic.Services
{
    internal class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ItemDto> GetItem(int id)
        {
            var item = await _unitOfWork.Repository<Item>().Entities.FirstOrDefaultAsync(w => w.Id == id);

            return _mapper.Map<Item, ItemDto>(item);
        }

        public async Task<List<ItemDto>> GetItems()
        {
            var items = await _unitOfWork.Repository<Item>().Entities.Include(w => w.Warehouse).ToListAsync();

            return _mapper.Map<List<Item>, List<ItemDto>>(items);
        }

        public async Task<PaginatedList<ItemDto>> GetItemsPaged(ItemQueryOption queryOption)
        {
            var query = _unitOfWork.Repository<Item>().Entities.Include(w => w.Warehouse)
                .Where(i => (queryOption.WarehouseId == null || i.WarehouseId == queryOption.WarehouseId) &&
                 (string.IsNullOrEmpty(queryOption.Name) || i.Name == queryOption.Name));

            var items = await PaginatedList<Item>.CreateAsync(query, queryOption.Page, queryOption.Size);
            return _mapper.Map<PaginatedList<Item>, PaginatedList<ItemDto>>(items);
        }

        public async Task<QueryResult<bool>> AddItem(ItemDto itemDto)
        {
            var existItem = await _unitOfWork.Repository<Item>().Entities.FirstOrDefaultAsync(w => w.WarehouseId == itemDto.WarehouseId && w.Name == itemDto.Name);
            if (existItem != null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "Item already exist.",
                };
            }

            var item = _mapper.Map<ItemDto, Item>(itemDto);
            await _unitOfWork.Repository<Item>().AddAsync(item);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }

        public async Task<QueryResult<bool>> UpdateItem(ItemDto itemDto)
        {
            var existItem = await _unitOfWork.Repository<Item>().Entities.FirstOrDefaultAsync(w => w.Id != itemDto.Id && w.WarehouseId == itemDto.WarehouseId && w.Name == itemDto.Name);
            if (existItem != null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "Item already exist.",
                };
            }

            var item = _mapper.Map<ItemDto, Item>(itemDto);
            await _unitOfWork.Repository<Item>().UpdateAsync(item);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }

        public async Task<QueryResult<bool>> DeleteItem(int id)
        {
            var item = await _unitOfWork.Repository<Item>().Entities.FirstOrDefaultAsync(w => w.Id == id);
            if (item == null)
            {
                return new QueryResult<bool>()
                {
                    Success = false,
                    Message = "Item NOT found.",
                };
            }

            await _unitOfWork.Repository<Item>().DeleteAsync(item);

            return new QueryResult<bool>()
            {
                Success = true
            };
        }
    }
}
