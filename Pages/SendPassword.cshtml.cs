using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2021IMMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2021IMMS.Pages
{
    public class SendPasswordModel : PageModel
    {
        private readonly IEmailService IEmailService;
        public SendPasswordModel(IEmailService IES)
        {
            IEmailService = IES;
        }

        public async Task<RedirectResult> OnGetAsync()
        {
            // Configure the email message and send it.
            string strToName = HttpContext.Session.GetString("strToName");
            string strToAddress = HttpContext.Session.GetString("strToAddress");
            string strPassword = HttpContext.Session.GetString("strPassword");
            string strSubject = "IMMS Pasword";
            string strBody = "Dear " + strToName + "," + "<br /><br /> Here is your password for the IMMS: " + strPassword
                + "<br /><br /> Thank you, <br/> <br/>The IMMS.";
            await IEmailService.SendEmail(strToName, strToAddress, strSubject, strBody);
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "lightgreen");
            HttpContext.Session.SetString("Message", "Your password has been sent to your email.");

            return Redirect ("/Login");
        }
    }
}
