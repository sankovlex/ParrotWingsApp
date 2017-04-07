using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParrotWings.WebAPI.ViewModels.Account
{
    public class UserRegisterPostModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "do not match password")]
        public string ConfirmPassword { get; set; }
    }
}
