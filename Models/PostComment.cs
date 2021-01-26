using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace MyBlog.Models
{
    public class PostComment
    {
        public int Id { get; set; }

        public int CategoryPostId { get; set; }

        public string BlogUserId { get; set; }

        public string CommentBody { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }
        
        public DateTime? Moderated { get; set; }
        
        public string ModReason { get; set; }

        public string ModBody { get; set; }


        //Nav
        public virtual CategoryPost CategoryPost { get; set; }
        public virtual BlogUser BlogUser { get; set; }



    }
}
