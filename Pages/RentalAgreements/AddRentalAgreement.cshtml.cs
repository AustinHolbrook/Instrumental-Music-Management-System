using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using _2021IMMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace _2021IMMS.Pages.RentalAgreements
{
    [BindProperties]
    public class AddRentalAgreementModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public AddRentalAgreementModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public SelectList StudentSelectList;
        public SelectList InstrumentSelectList;
        public IList<Student> StudentIList;
        public IList<Instrument> InstrumentIList;

        public RentalAgreement RentalAgreement { get; set; }

        public async Task OnGetAsync()
        {
            //Initialize the header data.
            ViewData["Page"] = "Add RentalAgreement";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Add.";

            // Populate the student select list.
            IList StudentIList = await (
                from s in IMMSContext.Student
                orderby s.LastName
                select new
                {
                    StudentID = s.StudentId,
                    Student = $"{s.LastName}, {s.FirstName} {s.MiddleInitial}",
                })
                .AsNoTracking().ToListAsync();
            // Insert values into ddl.
            StudentSelectList = new SelectList(StudentIList, "StudentID", "Student");

            // Populate the instrument select list.
            IList InstrumentIList = await (
                from i in IMMSContext.Instrument
                orderby i.Instrument1
                select new
                {
                    InstrumentID = i.InstrumentId,
                    Instrument = $"{i.Instrument1} ({i.SerialNumber})"
                })
                .AsNoTracking().ToListAsync();
            // Insert values into ddl.
            InstrumentSelectList = new SelectList(InstrumentIList, "InstrumentID", "Instrument");
        }

        public async Task<RedirectResult> OnPostAddAsync()
        {
            if (ModelState.IsValid)
            {
                // See if the email address entered is already associated with another account.
                if (!IMMSContext.RentalAgreement.Any(r => r.InstrumentId == RentalAgreement.InstrumentId))
                {
                    try
                    {
                        // Try to add the RentalAgreement to the database.
                        IMMSContext.RentalAgreement.Add(RentalAgreement);
                        await IMMSContext.SaveChangesAsync();
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "lightgreen");
                        HttpContext.Session.SetString("Message", "Rental agreement successfully added.");
                        return Redirect("MaintainRentalAgreement");
                    }
                    catch (DbUpdateException objDbUpdateException)
                    {
                        // An exception has occurred. Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", 
                            "Rental Agreement NOT added. Please report this message to: rbeasley@franklincollege.edu." 
                            + objDbUpdateException.InnerException.Message);
                        return Redirect("MaintainRentalAgreement");
                    }
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", 
                        "The rental agreement was NOT added because there is an active rental agreement for the selected instrument."
                        + " To add this rental agreement you must first delete the active rental agreement for this instrument.");
                    return Redirect("MaintainRentalAgreement");
                }
            }
            return Redirect("MaintainRentalAgreement");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "The operation was cancelled. No data was affected.");
            return Redirect("MaintainRentalAgreement");
        }
    }
}
