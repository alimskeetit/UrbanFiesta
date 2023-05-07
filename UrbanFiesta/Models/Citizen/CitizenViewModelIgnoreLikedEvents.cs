using System.Text.Json.Serialization;
using UrbanFiesta.Models.Event;

namespace UrbanFiesta.Models.Citizen
{
    public class CitizenViewModelIgnoreLikedEvents
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string[] Roles { get; set; }
    }
}
