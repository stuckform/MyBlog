using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlog.Data;
using MyBlog.Models;
using Npgsql;

namespace MyBlog.Utilities
{
    //In order to use an instance of this class (as it's defined right now..)
    //I would need the following code somewhere in my application


    public static class DataUtility
    {
        public static string GetConnectionString(IConfiguration configuration) 
        {
            //The default connection string will come from appSettings like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //It will be automatically overwritten if we are running on Heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        private static string BuildConnectionString(string databaseUrl)
        {
            //Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };

            return builder.ToString();

        }
       
        public static async Task ManageDataAsync(IHost host)
        {
            //This Technique is used to obtain refrences to services that get registered in the
            //ConfigureServices method in the startup class
            using var svcScope = host.Services.CreateScope();
            var svcProvider = svcScope.ServiceProvider;

            // This dbContextSvc knows how to talk to the DB (aka _context)
            //Service 1: an Incstance of Application DbContext
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();

            //Service 2: An instance of RoleManager
            var roleManagerSvc = svcProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Service 3: an instance of UserManager
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<BlogUser>>();
            
            //This is the programmict equivalent to Update-Database       
            await dbContextSvc.Database.MigrateAsync();

            //step 1: Add a few Roles into the system (administrator & moderator)
            await SeedRolesAsync(roleManagerSvc);

            //Step 2: Add a few Users 
            await SeedUsersAsync(userManagerSvc);

            //Step 3: Assign a User to the Admin and moderator roles. 
            await AssignRolesAsync(userManagerSvc);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleSvc)
            
        {
            //write the code to seed a few Roles
            //Call upon the roleSvc to add a new Role
            await roleSvc.CreateAsync(new IdentityRole("Administrator"));
            await roleSvc.CreateAsync(new IdentityRole("Moderator"));

        }

        private static async Task SeedUsersAsync(UserManager<BlogUser> userManagerSvc)
        {
            //Write the code to see a few Users
            //Step 1: Create yourself as a user
            var adminUser = new BlogUser()
            {
                Email = "Coppinger.dev@gmail.com",
                UserName = "Coppinger.dev@gmail.com",
                FirstName = "Matthew",
                LastName = "Coppinger",
                EmailConfirmed = true
            };

            await userManagerSvc.CreateAsync(adminUser, "Num1811418!");

            //Step 2 : Create Someone else as a user
            var modUser = new BlogUser()
            {
                Email = "Mod@mail.com",
                UserName = "Mod@mail.com",
                FirstName = "Maud",
                LastName = "Mod",
                EmailConfirmed = true
            };

            await userManagerSvc.CreateAsync(modUser, "Abc&123!");
        }

        private static async Task AssignRolesAsync(UserManager<BlogUser> userManagerSvc)
        {
            //Step 1: Somehow get a reference to the Coppinger.dev user
            var adminUser = await userManagerSvc.FindByEmailAsync("Coppinger.dev@gmail.com");

            //Step 2: Assign the adminUser to the Administrator role
            await userManagerSvc.AddToRoleAsync(adminUser, "Administrator");

            //Step 3: step 1 and 2 again but for moderator
            var modUser = await userManagerSvc.FindByEmailAsync("Mod@mail.com");
            await userManagerSvc.AddToRoleAsync(modUser, "Moderator");
        }


    }
}
