using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace _2021IMMS.Services
{
    public class EmailServiceMailKit : IEmailService
    {
        private readonly IConfiguration IConfiguration;
        public EmailServiceMailKit(IConfiguration IC)
        {
            IConfiguration = IC;
        }
        public async Task SendEmail(string strToName, string strToAddress, string strSubject, string StrBody)
        {
            // Build the email message.
            MimeMessage objMimeMessage = new MimeMessage();
            objMimeMessage.From.Add(new MailboxAddress("No Reply", "noreply@IMMS.com"));
            objMimeMessage.To.Add(new MailboxAddress(strToName, strToAddress));
            objMimeMessage.Subject = strSubject;
            objMimeMessage.Body = new TextPart(TextFormat.Html) { Text = StrBody };
            // Send the email message.
            SmtpClient objSmtpClient = new SmtpClient();
            string strHost = IConfiguration.GetValue<string>("Email:Host");
            int intPort = IConfiguration.GetValue<int>("Email:Port");
            await objSmtpClient.ConnectAsync(strHost, intPort);
            await objSmtpClient.SendAsync(objMimeMessage);
            await objSmtpClient.DisconnectAsync(true);
        }

    }
}
