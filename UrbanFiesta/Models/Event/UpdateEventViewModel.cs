using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class UpdateEventViewModel: CommandEventViewModel
    {
        [Required]
        [JsonPropertyOrder(0)]
        public int Id { get; set; }
    }
}
