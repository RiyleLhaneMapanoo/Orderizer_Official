using Microsoft.Extensions.Configuration;
using Ord_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace Ord_Business
{
    public class EmailService 
    {
        private readonly string _smtpServer ;
        private readonly int _port;
        private readonly string _username ;
        private readonly string _password;

        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;

            _smtpServer = _config["EmailSettings:SmtpHost"];
            _port = int.Parse(_config["EmailSettings:SmtpPort"]);
            _username = _config["EmailSettings:SmtpUser"];
            _password = _config["EmailSettings:SmtpPass"];



        }


        public void SendEmail(string Receiver, string subject,List<items> allItems)
        {
      
            //remove comment once in text, db data i
            //if (allItems == null || allItems.Count == 0)
            //{
            //    Console.WriteLine("⚠️ No items found to send.");
            //    return;
            //}

            string bodyHtml = @"
<html>
  <head>
    <style>
      body {
        font-family: 'Segoe UI', Arial, sans-serif;
        background-color: #f4f4f4;
        color: #333;
        margin: 0;
        padding: 30px;
      }
      .card {
        background-color: #ffffff;
        width: 600px;
        margin: 0 auto;
        padding: 40px;
        border-radius: 10px;
        box-shadow: 0 2px 6px rgba(0,0,0,0.15);
      }
      h1 {
        text-align: center;
        color: #2b2b2b;
        font-size: 26px;
        margin-bottom: 10px;
      }
      hr {
        border: none;
        border-top: 1px solid #ccc;
        margin: 15px 0 25px 0;
      }
      h2 {
        color: #333;
        margin-bottom: 15px;
      }
      ul {
        list-style-type: disc;
        margin-left: 25px;
      }
      li {
        margin-bottom: 8px;
        font-size: 16px;
      }
      .footer {
        font-style: italic;
        color: #777;
        font-size: 12px;
        margin-top: 25px;
        text-align: left;
      }
    </style>
  </head>
  <body>
    <div class='card'>
      <h1>OrderIZER Best Deals!</h1>
      <hr />
      <h2>🛍 Shopping List:</h2>
      <ul>
";

       
            foreach (var item in allItems)
            {
                if (item.Platforms != null && item.Platforms.Count > 0)
                {
                    var best = item.Platforms.OrderBy(p => p.Price).First();
                    bodyHtml += $"<li><b>{item.ItemName}</b> — {best.PlatformName}: ₱{best.Price:N2}</li>";
                }
            }

            bodyHtml += $@"
      </ul>
      <div class='footer'>
        Generated on {DateTime.Now:dddd, dd MMMM yyyy hh:mm tt}
      </div>
    </div>
  </body>
</html>";





            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_config["EmailSettings:FromName"], _config["EmailSettings:FromEmail"]));
            email.To.Add(new MailboxAddress(Receiver, Receiver));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = bodyHtml
            };

            using (var smtp = new SmtpClient())
            {
             
                smtp.Connect(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_username, _password);
                smtp.Send(email);
                smtp.Disconnect(true);
                
            }
        }

    }
}
