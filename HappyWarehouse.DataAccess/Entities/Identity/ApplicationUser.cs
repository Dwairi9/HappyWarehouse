using Microsoft.AspNetCore.Identity;

namespace HappyWarehouse.DataAccess.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public bool Active { get; set; }
    }
}
