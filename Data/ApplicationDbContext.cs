using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlogCategory> BlogCategory { get; set; }
        public DbSet<CategoryPost> CategoryPost { get; set; }
        public DbSet<MyBlog.Models.PostComment> PostComment { get; set; }
        public DbSet<MyBlog.Models.Tag> Tag { get; set; }
    }
}
