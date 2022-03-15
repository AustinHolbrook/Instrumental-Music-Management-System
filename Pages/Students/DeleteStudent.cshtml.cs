using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Students
{
    [BindProperties]
    public class DeleteStudentModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public DeleteStudentModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int intStudentId)
        {
            // Lookup the row in the table to see if it still exists.
            Student = await IMMSContext.Student
                .FindAsync(intStudentId);

            if (Student != null)
            {
                try
                {
                    // Delete the row from the table.
                    IMMSContext.Student.Remove(Student);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName + " was successfully deleted.");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception has occured.
                    SqlException objSqlException = objDbUpdateException.InnerException as SqlException;
                    if(objSqlException.Number == 547)
                    {
                        // A foreign key contraint database exception occurred.
                        // Set the message.
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName 
                            + " was NOT deleted because he/she is associated with one or more rental agreements. To delete this student, " 
                            + "you must first delete the associated rental agreement(s).");
                    }
                    else
                    {
                    // A database exception has occured. Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName 
                        + " was NOT deleted. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                    }
                }
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Student.FirstName + " " + Student.LastName + " was successfully deleted.");
            }
            return Redirect("MaintainStudent");
        }
    }
}
