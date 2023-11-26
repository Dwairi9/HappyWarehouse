using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.BusinessLogic.DTOs.QueryOptions;
using HappyWarehouse.BusinessLogic.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
	{
		private readonly IWarehouseService _warehouseService;

		public WarehouseController(IWarehouseService warehouseService)
		{
			_warehouseService = warehouseService;
		}

        [HttpGet("GetWarehouses")]
        public async Task<IActionResult> GetWarehouses()
        {
            return Ok(await _warehouseService.GetWarehouses());
        }

        [HttpGet("GetWarehousesPaged")]
        public async Task<IActionResult> GetWarehousesPaged(int? page = 1)
		{
			return Ok(await _warehouseService.GetWarehousesPaged(new QueryOption() {Page = (int)page }));
		}

		[HttpPost("Add")] 
        public async Task<IActionResult> AddWarehouse(WarehouseDto warehouseDto)
		{
			return Ok(await _warehouseService.AddWarehouse(warehouseDto));
		}

		[HttpPut("Update")] 
        public async Task<IActionResult> UpdateWarehouse(WarehouseDto warehouseDto)
		{
			return Ok(await _warehouseService.UpdateWarehouse(warehouseDto));
		}

		[HttpDelete("Delete/{id:int}")] 
        public async Task<IActionResult> DeleteWarehouse(int id)
		{
			return Ok(await _warehouseService.DeleteWarehouse(id));
		}
	}
}
