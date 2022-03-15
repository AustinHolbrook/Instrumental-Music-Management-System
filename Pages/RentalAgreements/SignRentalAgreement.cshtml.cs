using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.RentalAgreements
{
    [BindProperties]
    public class SignRentalAgreementModel : PageModel
    {
        public string RenterSignature { get; set; }

        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public SignRentalAgreementModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public class JoinResult
        {
            public int RentalAgreementId;
            public string Student;
            public int StudentId;
            public string Instrument;
            public int InstrumentId;
            public string SerialNumber;
            public string Brand;
            public int BrandId;
            public string AdditionalParts;
        }

        public IList<JoinResult> JoinResultIList;
        private IQueryable<JoinResult> JoinResultIQueryable;

        public RentalAgreement RentalAgreement { get; set; }

        public async Task<IActionResult> OnGetAsync(int intRentalAgreementId)
        {
            //Initialize the header data.
            ViewData["Page"] = "Sign Rental Agreement";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please read and verify the information, then click Sign.";

            await RetrieveRowsForDisplay(intRentalAgreementId);

            // Insert the current students name into the disabled textbox.
            // This way the logged in user can only sign there name.
            RenterSignature = HttpContext.Session.GetString("User");


            // Attempt to retrieve the row from the table.
            RentalAgreement = await IMMSContext.RentalAgreement
                .Where(a => a.RentalAgreementId == intRentalAgreementId)
                .FirstOrDefaultAsync();
            if (RentalAgreement != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "The selected RentalAgreement was recently deleted by someone else.");
                return Redirect("StudentRentalAgreements");
            }
        }

        private async Task RetrieveRowsForDisplay(int intRentalAgreementId)
        {
            // Retrieve the rows for display.
            JoinResultIQueryable = (
               from r in IMMSContext.RentalAgreement
               join s in IMMSContext.Student
                   on r.StudentId equals s.StudentId
               join i in IMMSContext.Instrument
                   on r.InstrumentId equals i.InstrumentId
               join b in IMMSContext.Brand
                   on i.BrandId equals b.BrandId
                   orderby i.Instrument1
               select new JoinResult
               {
                   RentalAgreementId = r.RentalAgreementId,
                   Student = s.FirstName + " " + s.MiddleInitial + ". " + s.LastName,
                   StudentId = s.StudentId,
                   Instrument = i.Instrument1,
                   InstrumentId = i.InstrumentId,
                   SerialNumber = i.SerialNumber,
                   Brand = b.Brand1,
                   BrandId = b.BrandId,
                   AdditionalParts = i.AdditionalParts
               })
               .AsNoTracking();

            // If a filter value was entered, modify the database query.
            if (intRentalAgreementId != 0)
            {
                // Filter results on selected student.
                JoinResultIQueryable = JoinResultIQueryable
                    .Where(jr => jr.RentalAgreementId == intRentalAgreementId);
            }
            
            // Retrieve the rows for display.
            JoinResultIList = await JoinResultIQueryable
            .ToListAsync();
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
                try
                {
                // Set the faculty signature equal to the current student.
                RentalAgreement.RenterSignature = HttpContext.Session.GetString("User");

                // Modify the row in the table.
                IMMSContext.RentalAgreement.Update(RentalAgreement);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", "Rental agreement successfully signed.");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception has occured. Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "Rental agreement NOT signed. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                }
                return Redirect("StudentRentalAgreements");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Operation was canceled. No data was affected.");
            return Redirect("StudentRentalAgreements");
        }
    }
}
