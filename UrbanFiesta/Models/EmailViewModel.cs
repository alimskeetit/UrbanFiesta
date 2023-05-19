using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models
{
    public class EmailViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
