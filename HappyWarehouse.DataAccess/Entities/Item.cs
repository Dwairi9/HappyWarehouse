using System.ComponentModel.DataAnnotations.Schema;

namespace HappyWarehouse.DataAccess.Entities
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Qty { get; set; }
        public decimal CostPrice { get; set; }
        public decimal MSRPPrice{ get; set; }
        public int WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public  virtual Warehouse Warehouse { get; set; }
    }
}
