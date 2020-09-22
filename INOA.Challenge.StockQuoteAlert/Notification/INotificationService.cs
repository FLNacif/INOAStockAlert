using System;
using System.Collections.Generic;
using System.Text;

namespace INOA.Challenge.StockQuoteAlert.Notification
{
    public interface INotificationService
    {

        void Notify(string title, string content);
    }
}
