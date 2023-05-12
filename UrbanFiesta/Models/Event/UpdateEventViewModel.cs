using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class UpdateEventViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string StartDate { get; set; }

        public string? EndDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Coordinates { get; set; }

        public EventStatus Status { get; set; } = EventStatus.NotStarted;
    }
}
