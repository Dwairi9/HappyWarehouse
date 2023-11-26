using System.ComponentModel.DataAnnotations;
using HappyWarehouse.BusinessLogic.DTOs.Common;

namespace HappyWarehouse.BusinessLogic.DTOs
{
    public class WarehouseDto : EntityDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string? City { get; set; }
        public string? CountryName { get; set; }
        [Required]
        public int CountryId { get; set; }
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
    }
}
