using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.RentalAgreements
{
    [BindProperties]
    public class StudentRentalAgreementsModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public StudentRentalAgreementsModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

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
            //Initialize the header data.
            ViewData["Page"] = "Student Rental Agreements";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Get the studentId of current user for querying the list.
            int StudentId = (int)HttpContext.Session.GetInt32("StudentID");

            // Define the database query.
            JoinResultIQueryable = (
               from r in _2021IMMSContext.RentalAgreement
               join s in _2021IMMSContext.Student
                     on r.StudentId equals s.StudentId
               join i in _2021IMMSContext.Instrument
                   on r.InstrumentId equals i.InstrumentId
               select new JoinResult
               {
                   Student = s.FirstName + " " + s.MiddleInitial + ". " + s.LastName,
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

            // Filter the list on the current student logged in.
            JoinResultIQueryable = JoinResultIQueryable
                .Where(jr => jr.StudentId == StudentId);

            // Retrieve the rows for display.
            JoinResultIList = await JoinResultIQueryable.ToListAsync();

        }
    }
}
