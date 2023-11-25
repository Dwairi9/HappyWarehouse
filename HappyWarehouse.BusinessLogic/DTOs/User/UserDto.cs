using HappyWarehouse.BusinessLogic.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace HappyWarehouse.BusinessLogic.DTOs
{
    public class UserDto : EntityDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FullName { get; set; }
        public bool Active { get; set; }

        public string RoleId { get; set; }
    }
}
