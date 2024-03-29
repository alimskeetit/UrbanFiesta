﻿using System.ComponentModel.DataAnnotations;

namespace UrbanFiesta.Models.Citizen
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
