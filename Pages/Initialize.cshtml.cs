using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2021IMMS.Pages
{
    public class InitializeModel : PageModel
    {
        //In order to redirect the user to the login page we must type RedirectResult onGet().
        public RedirectResult OnGet()
        {
            //Logout the user.
            HttpContext.Session.Clear();

            //Initialize the header data.
            HttpContext.Session.SetString("User", "*");
            HttpContext.Session.SetString("Status", "*");
            HttpContext.Session.SetString("MessageColor", "lightgreen");
            HttpContext.Session.SetString("Message", "Please log in.");

            //Redirect to the login page.
            return Redirect("/Login");
        }
    }
}
