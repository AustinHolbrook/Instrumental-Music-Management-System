using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Brands
{
    [BindProperties]
    public class ModifyBrandModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public ModifyBrandModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Brand Brand { get; set; }

        public async Task<IActionResult> OnGetAsync(int intBrandId)
        {
            //Initialize the header data.
            ViewData["Page"] = "Modify Brand";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            Brand = await IMMSContext.Brand
                .Where(b => b.BrandId == intBrandId)
                .FirstOrDefaultAsync();
            if (Brand != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "The selected Brand was recently deleted by someone else.");
                return Redirect("MaintainBrand");
            }
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                IMMSContext.Brand.Update(Brand);
                await IMMSContext.SaveChangesAsync();
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Brand.Brand1 + " was successfully modified.");
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception has occured. Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", Brand.Brand1 + " was NOT modified. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
            }
            return Redirect("MaintainBrand");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Operation was canceled. No data was affected.");
            return Redirect("MaintainBrand");
        }
    }
}
