using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2021IMMS.Pages.Administrators
{
    [BindProperties]
    public class AddAdministratorModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public AddAdministratorModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Administrator Administrator { get; set; }

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Add Administrator";
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
                if (!IMMSContext.Administrator.Any(a => a.EmailAddress == Administrator.EmailAddress))
                {
                    try
                    {
                        // Try to add the administrator to the database.
                        IMMSContext.Administrator.Add(Administrator);
                        await IMMSContext.SaveChangesAsync();
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "lightgreen");
                        HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName + " was successfully added.");
                        return Redirect("MaintainAdministrator");
                    }
                    catch (DbUpdateException objDbUpdateException)
                    {
                        // An exception has occurred. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName + " was NOT added. Please report this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                        return Redirect("MaintainAdministrator");
                    }
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName
                        + " was NOT added because there is already an account accociated with the email address: "
                        + Administrator.EmailAddress + ".");
                    return Redirect("MaintainAdministrator");
                }
            }
            return Redirect("MaintainAdministrator");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "The operation was cancelled. No data was affected.");
            return Redirect("MaintainAdministrator");
        }
    }
}
