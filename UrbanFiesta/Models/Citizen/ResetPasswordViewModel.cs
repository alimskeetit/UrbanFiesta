using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Citizen
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
