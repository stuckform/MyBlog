using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MyBlog.Models
{
    public class CategoryPost
    {
        public int Id { get; set; }
        public int BlogCategoryId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }
        public bool IsReady { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; } = DateTime.Now;
        public string Slug { get; set; }


        public virtual BlogCategory BlogCategory { get; set; }

        public virtual ICollection<PostComment> PostComments { get; set; } =
            new HashSet<PostComment>();

        public virtual ICollection<CategoryPost> CategoryPosts { get; set; } =
           new HashSet<CategoryPost>();

        public virtual ICollection<Tag> Tags { get; set; } =
            new HashSet<Tag>();
    }
}
