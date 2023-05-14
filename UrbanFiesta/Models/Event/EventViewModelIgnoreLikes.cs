using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class EventViewModelIgnoreLikes : EventViewModel
    {
        private new ICollection<CitizenViewModelIgnoreLikedEvents>? Likes { get; set; }
    }
}
