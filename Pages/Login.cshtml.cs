using System;
using System.Collections.Generic;
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
    public class LoginModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;
        public LoginModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public string SessionStatus_Name { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
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
        }
        public async Task<RedirectResult> OnPostLogin()
        {
            if (radUser == "Faculty")
            {
                //Log in the user.
                Administrator = await IMMSContext.Administrator
                    .Where(a => a.EmailAddress == EmailAddress
                        && a.Password == Password)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (Administrator != null && Administrator.Status == "F")
                {
                    // Set the message.
                    string strUser = Administrator.FirstName + " " + Administrator.MiddleInitial + " " + Administrator.LastName;
                    HttpContext.Session.SetString("User", strUser);
                    HttpContext.Session.SetString("Status", "Faculty");
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", "You have logged in successfully! Welcome to IMMS!");
                    return Redirect("/Welcome");
                }
                else 
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "You have entered an invalid email address and password combination. Please try again.");
                    return Redirect("/Login");
                }
            }
            else if (radUser == "StudentWorker")
            {
                //Log in the user.
                Administrator = await IMMSContext.Administrator
                    .Where(a => a.EmailAddress == EmailAddress
                        && a.Password == Password)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (Administrator != null && Administrator.Status == "S")
                {
                    // Set the message.
                    string strUser = Administrator.FirstName + " " + Administrator.MiddleInitial + " " + Administrator.LastName;
                    HttpContext.Session.SetString("User", strUser);
                    HttpContext.Session.SetString("Status", "Student Worker");
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", "You have logged in successfully! Welcome to IMMS!");
                    return Redirect("/Welcome");
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "You have entered an invalid email address and password combination. Please try again.");
                    return Redirect("/Login");
                }
            }
            else if (radUser == "Student")
            {
                //Log in the user.
                Student = await IMMSContext.Student
                    .Where(s => s.EmailAddress == EmailAddress
                        && s.Password == Password)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (Student != null)
                {
                    // Set the message.
                    string strUser = Student.FirstName + " " + Student.MiddleInitial + " " + Student.LastName;
                    HttpContext.Session.SetString("User", strUser);
                    HttpContext.Session.SetString("Status", "Student");
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", "You have logged in successfully! Welcome to IMMS!");
                    HttpContext.Session.SetInt32("StudentID", Student.StudentId);
                    return Redirect("/Welcome");
                }
                else
                {
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", "You have entered an invalid email address and password combination. Please try again.");
                    return Redirect("/Login");
                }
            }
            else
            {
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "Please select your user type before logging in.");
                return Redirect("/Login");
            }
        }

        public RedirectResult OnPostForgotPassword()
        {
            // Set the message, and redirect the user to the forgot password page.
            HttpContext.Session.SetString("MessageColor", "lightgreen");
            HttpContext.Session.SetString("Message", "Please select your user type and enter your email address, then click Send Password.");

            // If they have an email already entered, put it into the texbox on Forgot Password page.
            if (EmailAddress != null)
            {
                HttpContext.Session.SetString("EmailAddress", EmailAddress);
            }
            else
            {
                HttpContext.Session.SetString("EmailAddress", "");
            }

            return Redirect("/ForgotPassword");
        }
    }
}
