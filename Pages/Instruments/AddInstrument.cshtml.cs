using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace _2021IMMS.Pages.Instruments
{
    [BindProperties]
    public class AddInstrumentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public AddInstrumentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public SelectList BrandSelectList;

        public Instrument Instrument { get; set; }

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Add Instrument";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Add.";

            // Populate the brand select list.
            BrandSelectList = new SelectList(IMMSContext.Brand
                .AsNoTracking()
                .OrderBy(b => b.Brand1), "BrandId", "Brand1");
        }

        public async Task<RedirectResult> OnPostAddAsync()
        {
            if (ModelState.IsValid)
            {
                // If left null, there are no additional parts.
                if (Instrument.AdditionalParts == null)
                {
                    Instrument.AdditionalParts = "None";
                }

                // See if the email address entered is already associated with another account.
                if (!IMMSContext.Instrument.Any(i => i.SerialNumber == Instrument.SerialNumber))
                {
                    try
                    {
                        // Try to add the Instrument to the database.
                        IMMSContext.Instrument.Add(Instrument);
                        await IMMSContext.SaveChangesAsync();
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "lightgreen");
                        HttpContext.Session.SetString("Message", Instrument.Instrument1 + " was successfully added.");
                        return Redirect("MaintainInstrument");
                    }
                    catch (DbUpdateException objDbUpdateException)
                    {
                        // An exception has occurred. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Instrument.Instrument1 + " was NOT added. Please report this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                        return Redirect("MaintainInstrument");
                    }
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Instrument.Instrument1
                        + " was NOT added because an instrument with the exact serial number " + Instrument.SerialNumber + " already exists."
                        + " To add this instrument, you must first delete the instrument with this serial number.");
                    return Redirect("MaintainInstrument");

                }
            }
            return Redirect("MaintainInstrument");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "The operation was cancelled. No data was affected.");
            return Redirect("MaintainInstrument");
        }
    }
}
