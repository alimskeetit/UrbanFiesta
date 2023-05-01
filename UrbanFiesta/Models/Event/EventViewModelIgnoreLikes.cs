using System.Text.Json.Serialization;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class EventViewModelIgnoreLikes: EventViewModel
    {
        [JsonIgnore]
        public new ICollection<CitizenViewModel>? Likes { get; set; }
    }
}
