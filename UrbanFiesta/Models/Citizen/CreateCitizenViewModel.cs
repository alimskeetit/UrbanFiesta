using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Citizen
{
    public class CreateCitizenViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Неправильно указан Email")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; }
        public string? Patronymic { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string BirthDate { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
