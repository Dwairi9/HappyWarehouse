using System.ComponentModel.DataAnnotations.Schema;

namespace HappyWarehouse.DataAccess.Entities
{
    public class Warehouse : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
