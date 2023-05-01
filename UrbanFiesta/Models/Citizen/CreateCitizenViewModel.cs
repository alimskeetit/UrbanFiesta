using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Citizen
{
    public class CreateCitizenViewModel
    {
        public string Email { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = null!;
    }
}
