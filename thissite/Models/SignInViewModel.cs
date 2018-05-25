using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace thissite.Models
{
    public class SignInViewModel
    {

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(5, ErrorMessage = "You need to make your username at least 5 letters.")]
        [System.ComponentModel.DataAnnotations.MaxLength(50)]
        public string UserName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; }
    }
}
