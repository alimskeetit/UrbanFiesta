using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Citizen : IdentityUser 
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Event>? LikedEvents { get; set; }
        public bool IsBanned { get; set; }
        [NotMapped]
        public string[] Roles { get; set; }
        public string EmailForNewsletter { get; set; }
        [MaxLength(6)]
        public string CodeForConfirmEmailForNewsletter { get; set; } 
        public bool IsSubscribed { get; set; } 
    }
}
