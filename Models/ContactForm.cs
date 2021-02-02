using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    [Keyless]
    public class ContactForm
    {

        [Required]
        [StringLength(50, ErrorMessage = "the {0} must be at least {2}) and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "the {0} must be at least {2}) and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "An valid e-mail is required.")]   
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

       
        [Phone]
        [Display(Name ="Phone Number")]
        public string PhoneNum { get; set; }

        [StringLength(100, ErrorMessage = "the {0} must be at least {2}) and at max {1} characters long.", MinimumLength = 4)]
        public string Subject { get; set; }

        [StringLength(600, ErrorMessage = "the {0} must be at least {2}) and at max {1} characters long.", MinimumLength = 3)]
        public string Body { get; set; }
    }
}
