using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MyBlog.Models
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "the {0} must be at least {2}) and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(70, ErrorMessage = "the {0} must be at least {2}) and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //how to get the user's full name
        [NotMapped]//-do not map it to the table
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        //Nav
        public virtual ICollection<PostComment> PostComments { get; set; } =
            new HashSet<PostComment>();

        //a more formal form lastnaem, first name
        //[NotMapped]
        //public string FormalName
        //{
        //    get
        //    {
        //        return $"{LastName}, {FirstName}";

    }   
}

