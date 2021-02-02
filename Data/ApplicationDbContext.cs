using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Models;


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
        public DbSet<PostComment> PostComment { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<MyBlog.Models.ContactForm> ContactForm { get; set; }
    }
}
