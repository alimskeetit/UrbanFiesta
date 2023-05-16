using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using UrbanFiesta.Models.Citizen;

namespace UrbanFiesta.Models.Event
{
    public class UpdateEventViewModel: CommandEventViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}
