using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace _2021IMMS.Pages.RentalAgreements
{
    public class MaintainRentalAgreementModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public MaintainRentalAgreementModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

        [BindProperty]
        public bool UnsignedFilter { get; set; }

        [BindProperty]
        public int StudentId { get; set; }

        public SelectList StudentSelectList;
        public class JoinResult
        {
            public string Student;
            public int StudentId;
            public string Instrument;
            public int InstrumentId;
            public string SerialNumber;
            public int RentalAgreementId;
            public string RenterSignature;
            public string FacultySignature;
            public DateTime Date;
        }

        public IList<JoinResult> JoinResultIList;
        private IQueryable<JoinResult> JoinResultIQueryable;

        public async Task OnGetAsync()
        {
            await PopulateSelectListAsync();
            await RetrieveRowsForDisplay();
        }

        public async Task OnPostFilterAsync()
        {
            await PopulateSelectListAsync();
            await RetrieveRowsForDisplay();
        }

        public async Task OnPostUnsignedFilterAsync()
        {
            // Set the UnsignedFilter to true.
            UnsignedFilter = true;
            await PopulateSelectListAsync();
            await RetrieveRowsForDisplay();
        }

        public RedirectResult OnPostClearFilter()
        {
            // Refresh the page so that the filters will clear.
            return Redirect("/RentalAgreements/MaintainRentalAgreement");
        }

        private async Task PopulateSelectListAsync()
        {
            // Populate the student select list.
            IList StudentIList = await(
                from s in _2021IMMSContext.Student
                orderby s.LastName
                select new
                {
                    StudentID = s.StudentId,
                    Student = $"{s.LastName}, {s.FirstName} {s.MiddleInitial}",
                })
                .AsNoTracking().ToListAsync();
            // Insert values into ddl.
            StudentSelectList = new SelectList(StudentIList, "StudentID", "Student");
        }
        private async Task RetrieveRowsForDisplay()
        {
            //Initialize the header data.
            ViewData["Page"] = "Maintain Rental Agreement";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Retrieve the rows for display.
            JoinResultIQueryable = (
               from r in _2021IMMSContext.RentalAgreement
               join s in _2021IMMSContext.Student
                   on r.StudentId equals s.StudentId
               join i in _2021IMMSContext.Instrument
                   on r.InstrumentId equals i.InstrumentId
               orderby s.LastName
               select new JoinResult
               {
                   Student =   s.LastName + ", " + s.FirstName + " " + s.MiddleInitial,
                   StudentId = s.StudentId,
                   Instrument = i.Instrument1,
                   InstrumentId = i.InstrumentId,
                   SerialNumber = i.SerialNumber,
                   RentalAgreementId = r.RentalAgreementId,
                   RenterSignature = r.RenterSignature,
                   FacultySignature = r.FacultySignature,
                   Date = r.Date.Value.Date
               })
               .AsNoTracking();

            // If a filter value was entered, modify the database query.
            if (StudentId != 0)
            {
                // Filter results on selected student.
                JoinResultIQueryable = JoinResultIQueryable
                    .Where(jr => jr.StudentId == StudentId);
            }
            if (UnsignedFilter == true)
            {
                // Filter results on if both signatures are present in rental agreement.
                JoinResultIQueryable = JoinResultIQueryable
                    .Where(jr => jr.RenterSignature == null || jr.FacultySignature == null);
            }

            // Retrieve the rows for display.
            JoinResultIList = await JoinResultIQueryable
                .ToListAsync();

        }
    }
}
