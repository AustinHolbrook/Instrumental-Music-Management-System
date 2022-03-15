using System.Linq;
using System.Threading.Tasks;
using _2021IMMS.Models;
using _2021IMMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _2021IMMS.Pages
{
    [BindProperties]
    public class ForgotPasswordModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;
        public ForgotPasswordModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public string EmailAddress { get; set; }

        public string radUser { get; set; }

        // Create an Administrator and Student object.
        private Administrator Administrator;
        private Student Student;

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Login";
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Autofill the email texbox if they already entered it on the login page.
            EmailAddress = HttpContext.Session.GetString("EmailAddress");
        }

        public async Task<RedirectResult> OnPostSendPasswordAsync()
        {
            // If EmailAddress is not null and they have selected thier user type, we can look them up in the database.
            if (EmailAddress != null)
            {
                if(radUser == "Faculty" || radUser == "StudentWorker")
                {
                // Look up the user.
                Administrator = await IMMSContext.Administrator
                    .Where(a => a.EmailAddress == EmailAddress)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                    // If the Administrator was found, create the email message.
                    if (Administrator != null)
                    {
                        HttpContext.Session.SetString("strToName", Administrator.FirstName + " " + Administrator.MiddleInitial + " " + Administrator.LastName);
                        HttpContext.Session.SetString("strToAddress", Administrator.EmailAddress);
                        HttpContext.Session.SetString("strPassword", Administrator.Password);
                        return Redirect("/SendPassword");
                    }
                    else
                    {
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", "You have entered an invalid email address. Please try again.");
                        return Redirect("/ForgotPassword");
                    }
                }
                else if(radUser == "Student")
                {
                // Look up the user.
                Student = await IMMSContext.Student
                    .Where(s => s.EmailAddress == EmailAddress)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                    if(Student != null)
                    {
                        // Look up the user.
                        HttpContext.Session.SetString("strToName", Student.FirstName + " " + Student.MiddleInitial + " " + Student.LastName);
                        HttpContext.Session.SetString("strToAddress", Student.EmailAddress);
                        HttpContext.Session.SetString("strPassword", Student.Password);
                        return Redirect("/SendPassword");
                    }
                    else
                    {
                        HttpContext.Session.SetString("MessageColor", "tomato");
                        HttpContext.Session.SetString("Message", "You have entered an invalid email address. Please try again.");
                        return Redirect("/ForgotPassword");
                    }
                }
                else if (radUser == null)
                {
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "Please select your user type.");
                    return Redirect("/ForgotPassword");
                }
                else
                {
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "You have entered an invalid email address. Please try again.");
                    return Redirect("/ForgotPassword");
                }
            }
            else
            {
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "Please enter an email address.");
                return Redirect("/ForgotPassword");
            }
        }

        public RedirectResult OnPostCancel()
        {
            HttpContext.Session.SetString("MessageColor", "lightgreen");
            HttpContext.Session.SetString("Message", "Please log in.");
            // Set the message.
            return Redirect("/Login");
        }
    }
}
