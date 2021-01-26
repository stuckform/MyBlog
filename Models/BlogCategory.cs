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
        public string Name { get; set; }
        public string Description { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        // as A Blog Category I am likely to have zero or more Category Post instances
        public virtual ICollection<CategoryPost> CategoryPosts { get; set; } =
            new HashSet<CategoryPost>();
    }
}
