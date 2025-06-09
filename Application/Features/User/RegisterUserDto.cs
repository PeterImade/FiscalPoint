using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(20, ErrorMessage = "First name cannot be more than 20 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required" )]
        [StringLength(20, ErrorMessage = "First name cannot be more than 20 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
