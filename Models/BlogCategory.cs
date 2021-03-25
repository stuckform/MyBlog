using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class BlogCategory
    {
        
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Category")]
        public string Name { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        [Display(Name = "Choose Image")]
        public byte[] ImageData { get; set; }             
        public string ContentType { get; set; }

        // as A Blog Category I am likely to have zero or more Category Post instances
        public virtual ICollection<CategoryPost> CategoryPosts { get; set; } =
            new HashSet<CategoryPost>();
    }
}
