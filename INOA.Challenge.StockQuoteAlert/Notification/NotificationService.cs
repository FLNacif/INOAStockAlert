using INOA.Challenge.Entities.Configuration;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INOA.Challenge.StockQuoteAlert.Notification
{
    public class NotificationService : INotificationService, IDisposable
    {
        private readonly SmtpClient _smtp;
        private readonly ProgramConfiguration _config;
        public NotificationService(ProgramConfiguration config)
        {
            _smtp = new SmtpClient();
            _smtp.Connect(config.Mail.Host, config.Mail.Port);
            _smtp.Authenticate(config.Mail.Username, config.Mail.Password);
            _config = config;
        }

        public void Dispose()
        {
            _smtp.Disconnect(true);
        }

        public void Notify(string title, string content)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_config.Mail.Sender);
            var to = _config.Mail.To.Select(to => MailboxAddress.Parse(to));
            email.To.AddRange(to);
            email.Subject = title;
            email.Body = new TextPart(TextFormat.Html) { Text = content };

            _smtp.Send(email);
        }
    }
}
