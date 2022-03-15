using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Instruments
{
    [BindProperties]
    public class DeleteInstrumentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public DeleteInstrumentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Instrument Instrument { get; set; }

        public async Task<IActionResult> OnGetAsync(int intInstrumentId)
        {
            // Lookup the row in the table to see if it still exists.
            Instrument = await IMMSContext.Instrument
                .FindAsync(intInstrumentId);

            if (Instrument != null)
            {
                try
                {
                    // Delete the row from the table.
                    IMMSContext.Instrument.Remove(Instrument);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", Instrument.Instrument1 + " was successfully deleted.");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception has occured.
                    SqlException objSqlException = objDbUpdateException.InnerException as SqlException;
                    if (objSqlException.Number == 547)
                    {
                        // A foreign key contraint database exception occurred.
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Instrument.Instrument1
                            + " was NOT deleted because it is associated with a rental agreement. To delete this instrument, "
                            + "you must first delete the assosiated rental agreement.");
                    }
                    else
                    {
                        // A database exception has occured. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Instrument.Instrument1
                            + " was NOT deleted. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                    }
                }
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Instrument.Instrument1 + " was successfully deleted.");
            }
            return Redirect("MaintainInstrument");
        }
    }
}
