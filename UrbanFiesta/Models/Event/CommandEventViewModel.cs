using Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Event
{
    public class CommandEventViewModel
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

        [Required] public double[] Coordinates { get; set; }
        public string PosterUrl { get; set; }
        public EventStatus Status { get; set; } = EventStatus.NotStarted;
    }
}
