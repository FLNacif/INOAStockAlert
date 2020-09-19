using INOA.Challenge.Entities;
using INOA.Challenge.IStockObservable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace INOA.Challenge.StockObservable
{
    public class StockQuoteApiObservable : IStockQuoteObservable
    {

        private List<StockQuoteSubscription> subscriptions;
        private List<StockInfo> stockInfos;

        public StockQuoteApiObservable()
        {
            subscriptions = new List<StockQuoteSubscription>();
        }

        public IDisposable Subscribe(IObserver<StockInfo> observer)
        {
            return Subscribe(observer, new List<string>());

        }

        public IDisposable Subscribe(IObserver<StockInfo> observer, string stockCode)
        {
            return Subscribe(observer, new List<string>() { stockCode });
        }

        public IDisposable Subscribe(IObserver<StockInfo> observer, List<string> stockCodes)
        {
            if (!subscriptions.Any(obs => obs.Observer == observer))
            {
                subscriptions.Add(new StockQuoteSubscription(observer, stockCodes));
                if (subscriptions.Count == 1)
                {
                    LookForStockPrice();
                }
            }
            else
            {
                subscriptions.Find(obs => obs.Observer == observer).StockCodesInterested = stockCodes;
            }
            return new Unsubscriber<StockInfo>(subscriptions, observer);
        }

        private void LookForStockPrice()
        {
            while (subscriptions.Any())
            {
                var stock = new StockInfo();

                stockInfos.Add(stock);
                foreach (var observer in subscriptions)
                {
                    if (!observer.StockCodesInterested.Any() || observer.StockCodesInterested.Contains(stock.StockCode))
                    {
                        observer.Observer.OnNext(stock);
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
