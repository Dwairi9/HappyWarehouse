using System.ComponentModel.DataAnnotations;

namespace HappyWarehouse.BusinessLogic.DTOs
{
    public class ChangePasswordDto
    {
        public int Id { get; set; }

        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
