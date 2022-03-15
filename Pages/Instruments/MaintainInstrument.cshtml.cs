using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Instruments
{
    public class MaintainInstrumentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public MaintainInstrumentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

        [BindProperty]
        public string Filter { get; set; }

        public class JoinResult
        {
            public int InstrumentId;
            public string Instrument;
            public string Brand;
            public string SerialNumber;
            public string Type;
            public string Condition;
            public string AdditionalParts;
        }

        public IList<JoinResult> JoinResultIList;
        private IQueryable<JoinResult> JoinResultIQueryable;
        
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
            return Redirect("/Instruments/MaintainInstrument");
        }

        private async Task RetrieveRowsForDisplay()
        {
            //Initialize the header data.
            ViewData["Page"] = "Maintain Instrument";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Retrieve the rows for display.
             JoinResultIQueryable = (
                from i in _2021IMMSContext.Instrument
                join b in _2021IMMSContext.Brand
                    on i.BrandId equals b.BrandId
                orderby i.Instrument1
                select new JoinResult
                {
                    InstrumentId = i.InstrumentId,
                    Instrument = i.Instrument1,
                    Brand = b.Brand1,
                    SerialNumber = i.SerialNumber,
                    Type = i.Type,
                    Condition = i.Condition,
                    AdditionalParts = i.AdditionalParts
                })
                .AsNoTracking();

            // If a filter value was entered, modify the database query.
            if (Filter != null)
            {
                JoinResultIQueryable = JoinResultIQueryable
                    .Where(jr => jr.Instrument.Contains(Filter));
            }
            // Retrieve the rows for display.
            JoinResultIList = await JoinResultIQueryable
                .ToListAsync();

            // Expand each abbreviation in the list for user readablility.
            foreach(var item in JoinResultIList)
            {
                switch (item.Type)
                {
                    case "C":
                        item.Type = "Concert";
                        break;
                    case "M":
                        item.Type = "Marching";
                        break;
                    case "S":
                        item.Type = "String";
                        break; 
                    default:
                        break;
                }
                switch (item.Condition)
                {
                    case "G":
                        item.Condition = "Good";
                        break;
                    case "F":
                        item.Condition = "Fair";
                        break;
                    case "P":
                        item.Condition = "Poor";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
