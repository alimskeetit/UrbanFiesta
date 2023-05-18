using System.Text.Json.Serialization;
using Entities.Models;
using UrbanFiesta.Models.Event;

namespace UrbanFiesta.Models.Citizen
{
    public class CitizenViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public string[] Roles { get; set; }
        public bool IsSubscribed { get; set; }

        public ICollection<EventViewModel>? LikedEvents { get; set; }
    }
}
