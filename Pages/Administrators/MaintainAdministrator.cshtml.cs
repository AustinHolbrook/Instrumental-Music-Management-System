using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Administrators
{
    public class MaintainAdministratorModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public MaintainAdministratorModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

        [BindProperty]
        public string Filter { get; set; }

        public IList<Administrator> AdministratorIList;
        private IQueryable<Administrator> AdministratorIQueryable;

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
            return Redirect("/Administrators/MaintainAdministrator");
        }
        private async Task RetrieveRowsForDisplay()
        {
            //Initialize the header data.
            ViewData["Page"] = "Maintain Administrator";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Define the database query.
            AdministratorIQueryable = (
                from a in _2021IMMSContext.Administrator
                select a)
                .AsNoTracking()
                .OrderBy(a => a.LastName);

            // If a filter value was entered, modify the database query.
            if (Filter != null)
            {
                AdministratorIQueryable = AdministratorIQueryable
                    .Where(ail => ail.LastName.Contains(Filter) || ail.FirstName.Contains(Filter));
            }
            // Retrieve the rows for display.
            AdministratorIList = await AdministratorIQueryable
                .ToListAsync();

            // Expand each abbreviation in the list for user readablility.
            foreach (var item in AdministratorIList)
            {
                switch (item.Status)
                {
                    case "S":
                        item.Status = "Student";
                        break;
                    case "F":
                        item.Status = "Faculty";
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
