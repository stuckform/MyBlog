using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyBlog.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        public void OnGet()
        {
            ViewData["HeaderImage"] = "/Img/nemuel-sereti-unsplash.jpg";
            ViewData["HeaderText"] = "Reset Password";

        }
    }
}
