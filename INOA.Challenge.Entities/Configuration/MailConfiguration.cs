using System;
using System.Collections.Generic;
using System.Text;

namespace INOA.Challenge.Entities.Configuration
{
    public class MailConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] To { get; set; }
        public string Sender { get; set; }
    }
}
