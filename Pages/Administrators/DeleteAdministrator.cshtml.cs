using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Administrators
{
    [BindProperties]
    public class DeleteAdministratorModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public DeleteAdministratorModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Administrator Administrator { get; set; }

        public async Task<IActionResult> OnGetAsync(int intAdministratorId)
        {
            // Lookup the row in the table to see if it still exists.
            Administrator = await IMMSContext.Administrator
                .FindAsync(intAdministratorId);

            if(Administrator != null)
            {
                try
                {
                    // Delete the row from the table.
                    IMMSContext.Administrator.Remove(Administrator);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName 
                        + " was successfully deleted.");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception has occured. Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName 
                        + " was NOT deleted. Please report this this message to: rbeasley@franklincollege.edu." 
                        + objDbUpdateException.InnerException.Message);
                }
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Administrator.FirstName + " " + Administrator.LastName 
                    + " was successfully deleted.");
            }
            return Redirect("MaintainAdministrator");
        }
    }
}
