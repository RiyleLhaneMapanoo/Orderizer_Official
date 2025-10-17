using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Ord_EmailIntegration
{
    public class MailTrapService : IEmailService
    {
        private readonly string _smtpServer = "sandbox.smtp.mailtrap.io";
        private readonly int _port = 2525;
        private readonly string _username = "35cf89ccdceaff";
        private readonly string _password = "8dfbfc50a7b2d0";

        public void SendEmail(string to, string subject, string bodyHtml)
        {
            var message = new MailMessage();
            message.From = new MailAddress("noreply@orderizer.com", "OrderIZER System");
            message.To.Add(to);
            message.Subject = subject;
            message.Body = bodyHtml;
            message.IsBodyHtml = true;

            using (var client = new SmtpClient(_smtpServer, _port))
            {
                client.Credentials = new NetworkCredential(_username, _password);
                client.EnableSsl = true;
                client.Send(message);
                Console.WriteLine("✅ Email sent successfully!");
            }
        }

    }
}
