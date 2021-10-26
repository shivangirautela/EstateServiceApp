using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email Address is required.")]
        [Display(Name = "Please Enter Your Email Address")]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        [Key]
        public string Email { get; set; }

        [Display(Name = "Please Enter Your Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required]
        public long PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="Password does not match.")]     
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string RoleName { get; set; }

    }
}
