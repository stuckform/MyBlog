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

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Abstract { get; set; }

        [Required]
        [Display(Name = "Body")]
        public string PostBody { get; set; }

        [Display(Name = "Publish")]
        public bool IsReady { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public string Slug { get; set; }

        //I need to add properties for storing images
        [Display(Name = "Choose Image")]
        public byte[] ImageData { get; set;}
        public string ContentType { get; set; }



        [Display(Name = "Category")]
        public virtual BlogCategory BlogCategory { get; set; }

        public virtual ICollection<PostComment> PostComments { get; set; } =
            new HashSet<PostComment>();

        public virtual ICollection<Tag> Tags { get; set; } =
            new HashSet<Tag>();
    }
}
