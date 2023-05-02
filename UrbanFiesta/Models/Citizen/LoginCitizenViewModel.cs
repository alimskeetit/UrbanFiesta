using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Citizen
{
    public class LoginCitizenViewModel
    {                     
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
