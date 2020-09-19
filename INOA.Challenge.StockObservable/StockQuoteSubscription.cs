using INOA.Challenge.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace INOA.Challenge.StockObservable
{
    public class StockQuoteSubscription
    {
        public IObserver<StockInfo> Observer;
        public List<string> StockCodesInterested;

        public StockQuoteSubscription(IObserver<StockInfo> observer, List<string> stockCode)
        {
            Observer = observer;
            StockCodesInterested = stockCode;
        }

        public StockQuoteSubscription(IObserver<StockInfo> observer, string stockCode)
        {
            Observer = observer;
            StockCodesInterested = new List<string>();
            StockCodesInterested.Add(stockCode);
        }

        public StockQuoteSubscription(IObserver<StockInfo> observer)
        {
            Observer = observer;
            StockCodesInterested = new List<string>();
        }
    }
}
