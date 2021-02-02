using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog.Utilities
{
    //In order to use an instance of this class (as it's defined right now..)
    //I would need the following code somewhere in my application


    public static class DataManager
    {
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
                Email = "Matt.kaizen@gmail.com",
                UserName = "Matt.kaizen@gmail.com",
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
            //Step 1: Somehow get a reference to the Matt.kaizen user
            var adminUser = await userManagerSvc.FindByEmailAsync("Matt.kaizen@gmail.com");

            //Step 2: Assign the adminUser to the Administrator role
            await userManagerSvc.AddToRoleAsync(adminUser, "Administrator");

            //Step 3: step 1 and 2 again but for moderator
            var modUser = await userManagerSvc.FindByEmailAsync("Mod@mail.com");
            await userManagerSvc.AddToRoleAsync(modUser, "Moderator");
        }


    }
}
