using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text.Json.Serialization;
using Entities.Enums;

namespace UrbanFiesta.Models.Event
{
    public class CreateEventViewModel: CommandEventViewModel
    {
        [JsonIgnore]
        public new int Id { get; set; }
    }
}
