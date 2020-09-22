using INOA.Challenge.Entities;
using INOA.Challenge.IStockObservable;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace INOA.Challenge.StockObservable.StockQuoteApiObservable
{
    public class StockQuoteApiObservable : IStockQuoteObservable
    {

        private List<StockQuoteSubscription> subscriptions;
        private readonly IStockQuoteApiService _stockApi;
        private readonly ILogger<StockQuoteApiObservable> _log;
        public StockQuoteApiObservable(IStockQuoteApiService stockApi, ILogger<StockQuoteApiObservable> log)
        {
            subscriptions = new List<StockQuoteSubscription>();
            _stockApi = stockApi;
            _log = log;
        }

        public IDisposable Subscribe(IObserver<IEnumerable<StockInfo>> observer)
        {
            return Subscribe(observer, new List<string>());
        }

        public IDisposable Subscribe(IObserver<IEnumerable<StockInfo>> observer, string stockCode)
        {
            return Subscribe(observer, new List<string>() { stockCode });
        }

        public IDisposable Subscribe(IObserver<IEnumerable<StockInfo>> observer, List<string> stockCodes)
        {
            stockCodes = stockCodes.Select(x => x.ToUpper()).ToList();
            if (!subscriptions.Any(obs => obs.Observer == observer))
            {
                subscriptions.Add(new StockQuoteSubscription(observer, stockCodes));
                if (subscriptions.Count == 1)
                {
                    Task.Run(() => LookForStockPrice());
                }
            }
            else
            {
                subscriptions.Find(obs => obs.Observer == observer).StockCodesInterested = stockCodes;
            }
            return new Unsubscriber<StockInfo>(subscriptions, observer);
        }

        private async void LookForStockPrice()
        {
            while (subscriptions.Any())
            {
                // Buscar dados da cotação
                var stockCodes = subscriptions.SelectMany(x => x.StockCodesInterested).Distinct();
                var stockQuotes = await _stockApi.BuscarCotacao(stockCodes);
                _log.LogInformation($"Novas cotações encontradas para os ativos {String.Join(", ", stockQuotes.Select(x => x.StockCode))}");
                foreach (var observer in subscriptions)
                {
                    var stockForSubscription = stockQuotes.Where(x => observer.StockCodesInterested.Contains(x.StockCode));
                    if (!observer.StockCodesInterested.Any() || stockQuotes.Any())
                    {
                        observer.Observer.OnNext(stockForSubscription);
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private StockInfo BuscarCotacao()
        {
            return new StockInfo();
        }
    }
}
