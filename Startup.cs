using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Services;
using MyBlog.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(DataUtility.GetConnectionString(Configuration)));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<BlogUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            //This is how I register a custom class as a service
            services.AddTransient<ISlugService, BasicSlugService>();

            //Register our new BasicImageService
            services.AddTransient<IImageService, BasicImageService>();

            //Services needed to send emails
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IEmailSender, EmailService>();

            //Api services

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Code Cache Blog",
                    Version = "v1",
                    Description = "This is a blog about programming and technology.",
                    Contact = new OpenApiContact
                    {
                        Email = "Coppinger.dev@gmail.com",
                        Name = "Matthew Coppinger",
                        Url = new Uri("https://mattcoppinger.netlify.app")
                    }
                });
            });
            //Service for 3rd party authentication
            services.AddAuthentication()
            .AddGitHub(options =>
            {
                options.ClientId = "a651803fe5065b67ac6a";
                options.ClientSecret = "bf6d8828e797756a6c7dae9e4020456923735f6a";
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            })
      
            .AddGoogle(options =>
            {
                options.ClientId = "691298918451-98b50du4gro5omo1v1dfq6iumf7ag8dq.apps.googleusercontent.com";
                options.ClientSecret = "uM33AIuveMU6XpzE68pe1Q7k";
                options.AccessDeniedPath = "/AccessDeniedPathInfo";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("DefaultPolicy");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Code Cache Blog");
                c.InjectJavascript("/swagger/swagger.js");
                c.InjectStylesheet("/swagger/swagger.css");
                c.DocumentTitle = "Code Cache Blog";

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                 name: "slug",
                 pattern: "Posts/URLFriendly/{slug}",
                 defaults: new { controller = "CategoryPosts", action = "Details" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapControllers();
            });
        }
    }
}
