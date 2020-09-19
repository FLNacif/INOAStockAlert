using INOA.Challenge.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace INOA.Challenge.StockObservable
{
    internal class Unsubscriber<BaggageInfo> : IDisposable
    {
        private List<StockQuoteSubscription> _subscriptions;
        private IObserver<StockInfo> _observer;

        internal Unsubscriber(List<StockQuoteSubscription> subscriptions, IObserver<StockInfo> observer)
        {
            this._subscriptions = subscriptions;
            this._observer = observer;
        }

        public void Dispose()
        {
            var observer = _subscriptions.FirstOrDefault(obs => obs.Observer == _observer);
            if (observer != null)
            {
                _subscriptions.Remove(observer);
            }
        }
    }
}
