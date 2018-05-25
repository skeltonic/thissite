using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace thissite.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "You need to make your username at least 5 letters.")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        // [Required]
        // [EmailAddress] Temporarily removing email validation for testing purposes
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}