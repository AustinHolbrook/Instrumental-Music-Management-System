using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Brands
{
    public class MaintainBrandModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public MaintainBrandModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

        [BindProperty]
        public string Filter { get; set; }


        public IList<Brand> BrandIList;
        private IQueryable<Brand> BrandIQueryable;

        public async Task OnGetAsync()
        {
            await RetrieveRowsForDisplay();
        }

        public async Task OnPostFilterAsync()
        {
            await RetrieveRowsForDisplay();
        }

        public RedirectResult OnPostClearFilter()
        {
            // Refresh the page so that the filter and textbox will clear.
            return Redirect("/Brands/MaintainBrand");
        }

        private async Task RetrieveRowsForDisplay()
        {
            //Initialize the header data.
            ViewData["Page"] = "Maintain Brand";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Define the database query.
            BrandIQueryable = (
                from b in _2021IMMSContext.Brand
                orderby b.Brand1
                select b)
                .AsNoTracking();

            // If a filter value was entered, modify the database query.
            if (Filter != null)
            {
                BrandIQueryable = BrandIQueryable
                    .Where(bil => bil.Brand1.Contains(Filter));
            }
            // Retrieve the rows for display.
            BrandIList = await BrandIQueryable
                .ToListAsync();
        }
    }
}
