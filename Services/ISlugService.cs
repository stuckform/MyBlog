using MyBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Services
{
    
     public interface ISlugService
    {
        
        string URLFriendly(string title);

        bool Isunique(ApplicationDbContext dbContext, string slug);
    }
}
