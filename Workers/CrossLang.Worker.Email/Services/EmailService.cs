using System;
using CrossLang.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace CrossLang.Worker.Email.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration config)
        {
            _configuration = config;
        }

        public void SendEmail(EmailMessage email)
        {
            var from = _configuration["Email:Username"];
            var psw = _configuration["Email:Password"];
            var host = _configuration["Email:Host"];
            var port = _configuration["Email:Port"];

            var sentEmail = new MimeMessage();

            sentEmail.From.Add(MailboxAddress.Parse(from));
            sentEmail.To.Add(MailboxAddress.Parse(email.To));
            sentEmail.Subject = email.Subject;
            sentEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = email.Body };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(host, int.Parse(port), MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(from, psw);
                smtp.Send(sentEmail);
                smtp.Disconnect(true);
            }
        }
    }
}

