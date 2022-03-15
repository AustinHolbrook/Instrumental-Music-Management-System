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

namespace _2021IMMS.Pages.Students
{
    [BindProperties]
    public class AddStudentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public AddStudentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Student Student { get; set; }

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Add Student";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Add.";
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
                        HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName + " was successfully added.");
                        return Redirect("MaintainStudent");
                    }
                    catch (DbUpdateException objDbUpdateException)
                    {
                        // An exception has occurred. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName + " was NOT added. Please report this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                        return Redirect("MaintainStudent");
                    }
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName 
                        + " was NOT added because there is already an account accociated with the email address: "
                        + Student.EmailAddress + ".");
                    return Redirect("MaintainStudent");
                }
            }
            return Redirect("MaintainStudent");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "The operation was cancelled. No data was affected.");
            return Redirect("MaintainStudent");
        }
    }
}
