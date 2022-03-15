using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Instruments
{
    [BindProperties]
    public class ModifyInstrumentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public ModifyInstrumentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public SelectList BrandSelectList;
        public Instrument Instrument { get; set; }

        public async Task<IActionResult> OnGetAsync(int intInstrumentId)
        {
            //Initialize the header data.
            ViewData["Page"] = "Modify Instrument";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Modify.";

            // Populate the brand select list.
            BrandSelectList = new SelectList(IMMSContext.Brand
                .AsNoTracking()
                .OrderBy(b => b.Brand1), "BrandId", "Brand1");

            // Attempt to retrieve the row from the table.
            Instrument = await IMMSContext.Instrument
                .Where(a => a.InstrumentId == intInstrumentId)
                .FirstOrDefaultAsync();
            if (Instrument != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "The selected Instrument was recently deleted by someone else.");
                return Redirect("MaintainInstrument");
            }
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                IMMSContext.Instrument.Update(Instrument);
                await IMMSContext.SaveChangesAsync();
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Instrument.Instrument1 + " was successfully modified.");
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception has occured. Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", Instrument.Instrument1 + " was NOT modified. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
            }
            return Redirect("MaintainInstrument");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Operation was canceled. No data was affected.");
            return Redirect("MaintainInstrument");
        }
    }
}
