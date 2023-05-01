using System.Text.Json.Serialization;
using Entities.Enums;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class EventViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string PosterUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public EventStatus Status { get; set; }
        
        public ICollection<CitizenViewModel>? Likes { get; set; }
    }
}
