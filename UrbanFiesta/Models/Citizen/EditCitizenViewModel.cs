using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Citizen
{
    public class EditCitizenViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string? Patronymic { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
