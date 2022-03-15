using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.RentalAgreements
{
    [BindProperties]
    public class DeleteRentalAgreementModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public DeleteRentalAgreementModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public RentalAgreement RentalAgreement { get; set; }

        public async Task<IActionResult> OnGetAsync(int intRentalAgreementId)
        {
            // Lookup the row in the table to see if it still exists.
            RentalAgreement = await IMMSContext.RentalAgreement
                .FindAsync(intRentalAgreementId);

            if (RentalAgreement != null)
            {
                try
                {
                    // Delete the row from the table.
                    IMMSContext.RentalAgreement.Remove(RentalAgreement);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", "Rental agreement successfully deleted.");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception has occured. Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "Rental agreement was NOT deleted. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                }
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", "Rental agreement was successfully deleted.");
            }
            return Redirect("MaintainRentalAgreement");
        }
    }
}
