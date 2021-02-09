using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly MailSettings _mailSettings;

        public HomeController(
            ILogger<HomeController> logger,
            IEmailSender emailSender,
            IOptions<MailSettings> mailsettings,
            ApplicationDbContext context,
            IImageService imageService)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _imageService = imageService;
            _mailSettings = mailsettings.Value;
        }

        public  async Task<IActionResult> Index()
        {
            var categories = await _context.BlogCategory.ToListAsync();
            return View(categories);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactForm contactForm)
        {
            await _emailSender.SendEmailAsync(_mailSettings.Mail, $"{contactForm.Subject} from {contactForm.FirstName} at {contactForm.Email}", contactForm.Body);

            return RedirectToAction("contact");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

