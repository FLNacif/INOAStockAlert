using INOA.Challenge.Entities;
using System;
using System.Collections.Generic;

namespace INOA.Challenge.IStockObservable
{
    public interface IStockQuoteObservable : IObservable<StockInfo>
    {
        IDisposable Subscribe(IObserver<StockInfo> observer, string stockCode);
        IDisposable Subscribe(IObserver<StockInfo> observer, List<string> stockCodes);
    }
}
