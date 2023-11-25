using System.ComponentModel.DataAnnotations;
using HappyWarehouse.BusinessLogic.DTOs.Common;

namespace HappyWarehouse.BusinessLogic.DTOs
{
    public class ItemDto : EntityDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        [Required]
        public decimal CostPrice { get; set; }
        
        [Required]
        public int WarehouseId { get; set; }

        public decimal MSRPPrice { get; set; }
        public string SKU { get; set; }
    }
}
