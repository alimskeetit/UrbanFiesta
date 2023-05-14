using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class EventViewModelIgnoreLikes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double[] Coordinates { get; set; }
        public string PosterUrl { get; set; }
        public EventStatus Status { get; set; }
    }
}
