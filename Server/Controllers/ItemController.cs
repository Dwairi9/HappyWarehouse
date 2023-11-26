using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.DTOs;
using Microsoft.AspNetCore.Mvc;
using HappyWarehouse.BusinessLogic.Services.IServices;

namespace HappyWarehouse.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemController : ControllerBase
	{
		private readonly IItemService _itemService;

		public ItemController(IItemService warehouseService)
		{
			_itemService = warehouseService;
		}

        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems()
		{
			return Ok(await _itemService.GetItems());
		}

        [HttpGet("GetItemsPaged")]
        public async Task<IActionResult> GetItemsPaged(int? page = 1, int? warehouseId = null)
        {
            return Ok(await _itemService.GetItemsPaged(new ItemQueryOption() { Page = (int)page, WarehouseId = warehouseId }));
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddItem(ItemDto warehouseDto)
		{
			return Ok(await _itemService.AddItem(warehouseDto));
		}

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateItem(ItemDto warehouseDto)
		{
			return Ok(await _itemService.UpdateItem(warehouseDto));
		}

        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> DeleteItem(int id)
		{
			return Ok(await _itemService.DeleteItem(id));
		}
	}
}
