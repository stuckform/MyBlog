using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MyBlog.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        //Nav
        public virtual ICollection<CategoryPost> CategoryPosts { get; set; } =
            new HashSet<CategoryPost>();
    }
}
