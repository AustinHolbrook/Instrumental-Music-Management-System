using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace _2021IMMS.Pages.Brands
{
    [BindProperties]
    public class AddBrandModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public AddBrandModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Brand Brand { get; set; }

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Add Brand";
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
                if (!IMMSContext.Brand.Any(b => b.Brand1 == Brand.Brand1))
                {

                    try
                    {
                        // Try to add the Brand to the database.
                        IMMSContext.Brand.Add(Brand);
                        await IMMSContext.SaveChangesAsync();
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "lightgreen");
                        HttpContext.Session.SetString("Message", Brand.Brand1 + " was successfully added.");
                        return Redirect("MaintainBrand");
                    }
                    catch (DbUpdateException objDbUpdateException)
                    {
                        // An exception has occurred. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Brand.Brand1 + " was NOT added. Please report this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                        return Redirect("MaintainBrand");
                    }
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Brand.Brand1
                        + " was NOT added because it already exists.");
                    return Redirect("MaintainBrand");
                }
            }
            return Redirect("MaintainBrand");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "The operation was cancelled. No data was affected.");
            return Redirect("MaintainBrand");
        }
    }
}
