using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Students
{
    public class MaintainStudentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public MaintainStudentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

        [BindProperty]
        public string Filter { get; set; }

        public IList<Student> StudentIList;
        private IQueryable<Student> StudentIQueryable;

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
            return Redirect("/Students/MaintainStudent");
        }

        private async Task RetrieveRowsForDisplay()
        {
            //Initialize the header data.
            ViewData["Page"] = "Maintain Student";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Retrieve the rows for display.
            StudentIQueryable = (
                from s in _2021IMMSContext.Student
                select s)
                .AsNoTracking()
                .OrderBy(s => s.LastName);

            // If a filter value was entered, modify the database query.
            if (Filter != null)
            {
                StudentIQueryable = StudentIQueryable
                    .Where(sil => sil.LastName.Contains(Filter) || sil.FirstName.Contains(Filter));
            }
            // Retrieve the rows for display.
            StudentIList = await StudentIQueryable
                .ToListAsync();
        }
    }
}
