using System;
using CrossLang.Models;

namespace CrossLang.Worker.Email.Services
{
    public interface IEmailService
    {
        public void SendEmail(EmailMessage email);
    }
}

