using System.ComponentModel.DataAnnotations;
using HappyWarehouse.BusinessLogic.DTOs.Common;

namespace HappyWarehouse.BusinessLogic.DTOs
{
    public class ItemDto : EntityDto
    {
        [Required]
        public string Name { get; set; }
        public string? WarehouseName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Qty Field is required.")]
        public int Qty { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Cost Price Field is required.")]
        public decimal CostPrice { get; set; }
        
        [Required]
        public int WarehouseId { get; set; }

        public decimal MSRPPrice { get; set; }
        public string? SKU { get; set; }
    }
}
