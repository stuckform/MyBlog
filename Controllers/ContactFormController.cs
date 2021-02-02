using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    public class ContactFormController : Controller
    {
        // GET: 
        public ActionResult Contact()
        {
            return View();
        }



        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      
        
    }
}
