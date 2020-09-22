using INOA.Challenge.Entities;
using System;
using System.Collections.Generic;

namespace INOA.Challenge.IStockObservable
{
    public interface IStockQuoteObservable : IObservable<IEnumerable<StockInfo>>
    {
        IDisposable Subscribe(IObserver<IEnumerable<StockInfo>> observer, string stockCode);
        IDisposable Subscribe(IObserver<IEnumerable<StockInfo>> observer, List<string> stockCodes);
    }
}
