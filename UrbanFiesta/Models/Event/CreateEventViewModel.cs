using System.ComponentModel.DataAnnotations;
using Entities.Enums;

namespace UrbanFiesta.Models.Event
{
    public class CreateEventViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string StartDate { get; set; }

        public string? EndDate { get; set; }

        public EventStatus Status { get; set; } = EventStatus.NotStarted;
    }
}
