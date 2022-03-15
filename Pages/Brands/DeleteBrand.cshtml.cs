using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Brands
{
    [BindProperties]
    public class DeleteBrandModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public DeleteBrandModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Brand Brand { get; set; }

        public async Task<IActionResult> OnGetAsync(int intBrandId)
        {
            // Lookup the row in the table to see if it still exists.
            Brand = await IMMSContext.Brand
                .FindAsync(intBrandId);

            if (Brand != null)
            {
                try
                {
                    // Delete the row from the table.
                    IMMSContext.Brand.Remove(Brand);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", Brand.Brand1 + " was successfully deleted.");
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
                        HttpContext.Session.SetString("Message", Brand.Brand1
                            + " was NOT deleted because it is associated with one or more instruments. To delete this brand, "
                            + "you must first delete the associated instruments.");
                    }
                    else
                    {
                        // A database exception has occured. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Brand.Brand1
                            + " was NOT deleted. Please report this this message to: rbeasley@franklincollege.edu." 
                            + objDbUpdateException.InnerException.Message);
                    }
                }
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Brand.Brand1 + " was successfully deleted.");
            }
            return Redirect("MaintainBrand");
        }
    }
}
