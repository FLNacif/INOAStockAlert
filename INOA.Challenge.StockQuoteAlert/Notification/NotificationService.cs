using INOA.Challenge.Entities.Configuration;
using System;
using System.Linq;
using System.Net.Mail;

namespace INOA.Challenge.StockQuoteAlert.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly SmtpClient _smtp;
        private readonly ProgramConfiguration _config;
        public NotificationService(ProgramConfiguration config)
        {
            _smtp = new SmtpClient();
            _smtp.Host = config.Mail.Host;
            _smtp.Port = config.Mail.Port;
            _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtp.EnableSsl = true;
            _smtp.Credentials = new System.Net.NetworkCredential(config.Mail.Sender, config.Mail.Password);
            _config = config;
        }

        public void Notify(string title, string content)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_config.Mail.Sender);
            foreach (var to in _config.Mail.To)
            {
                message.To.Add(to);
            }

            message.Subject = title;
            message.Body = content;

            _smtp.Send(message);
        }
    }
}
