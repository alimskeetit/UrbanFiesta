using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Enums;

namespace Entities.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        public string? PosterUrl { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Coordinates { get; set; }
        public EventStatus Status { get; set; }
        
        public ICollection<Citizen>? Likes { get; set; }
    }
}
