namespace UrbanFiesta.Models.Citizen
{
    public class EditCitizenViewModel
    {
        public string Email { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
