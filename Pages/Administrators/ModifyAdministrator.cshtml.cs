using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Administrators
{
    [BindProperties]
    public class ModifyAdministratorModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public ModifyAdministratorModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Administrator Administrator { get; set; }

        public async Task<IActionResult> OnGetAsync(int intAdministratorId)
        {
            //Initialize the header data.
            ViewData["Page"] = "Modify Administrator";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            Administrator = await IMMSContext.Administrator
                .Where(a => a.AdministratorId == intAdministratorId)
                .FirstOrDefaultAsync();
            if(Administrator != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "The selected administrator was recently deleted by someone else.");
                return Redirect("MaintainAdministrator");
            }
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                IMMSContext.Administrator.Update(Administrator);
                await IMMSContext.SaveChangesAsync();
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName + " was successfully modified.");
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception has occured. Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName + " was NOT modified. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
            }
            return Redirect("MaintainAdministrator");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Operation was canceled. No data was affected.");
            return Redirect("MaintainAdministrator");
        }
    }
}
