using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Students
{
    [BindProperties]
    public class ModifyStudentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public ModifyStudentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int intStudentId)
        {
            //Initialize the header data.
            ViewData["Page"] = "Modify Student";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            Student = await IMMSContext.Student
                .Where(s => s.StudentId == intStudentId)
                .FirstOrDefaultAsync();
            if (Student != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "The selected student was recently deleted by someone else.");
                return Redirect("MaintainStudent");
            }
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            try
            {
                // Modify the row in the table.
                IMMSContext.Student.Update(Student);
                await IMMSContext.SaveChangesAsync();
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName + " was successfully modified.");
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception has occured. Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName + " was NOT modified. Please report this this message to: rbeasley@franklincollege.edu" + objDbUpdateException.InnerException.Message);
            }
            return Redirect("MaintainStudent");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Operation was canceled. No data was affected.");
            return Redirect("MaintainStudent");
        }
    }
}
