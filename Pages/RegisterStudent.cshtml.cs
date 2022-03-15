using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _2021IMMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace _2021IMMS.Pages
{
    [BindProperties]
    public class RegisterStudentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public RegisterStudentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Student Student { get; set; }

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Register Student";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter your information below and click Register.";
        }

        public async Task<RedirectResult> OnPostAddAsync()
        {
            if (ModelState.IsValid)
            {
                // See if the email address entered is already associated with another account.
                if (!IMMSContext.Student.Any(s => s.EmailAddress == Student.EmailAddress))
                {
                    try
                    {
                        // Try to add the administrator to the database.
                        IMMSContext.Student.Add(Student);
                        await IMMSContext.SaveChangesAsync();
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "lightgreen");
                        HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName 
                            + " was successfully registered.");
                        return Redirect("Login");
                    }
                    catch (DbUpdateException objDbUpdateException)
                    {
                        // An exception has occurred. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName 
                            + " was NOT registered. Please report this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                        return Redirect("Login");
                    }
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "There is already an account associated with the email address: "
                        + Student.EmailAddress + ". Please click Forgot Password to retrieve the password for this account.");
                    return Redirect("Login");
                }
            }
            return Redirect("Login");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Registration cancelled.");
            return Redirect("Login");
        }
    }
}
