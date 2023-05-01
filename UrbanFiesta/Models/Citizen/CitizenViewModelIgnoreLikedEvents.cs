using System.Text.Json.Serialization;
using UrbanFiesta.Models.Event;

namespace UrbanFiesta.Models.Citizen
{
    public class CitizenViewModelIgnoreLikedEvents: CitizenViewModel
    {
        public ICollection<EventViewModelIgnoreLikes>? LikedEvents { get; set; }
    }
}
