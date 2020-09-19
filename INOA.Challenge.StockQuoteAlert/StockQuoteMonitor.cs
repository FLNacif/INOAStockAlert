using INOA.Challenge.Entities;
using INOA.Challenge.IStockObservable;
using Microsoft.Extensions.Logging;
using System;

namespace INOA.Challenge.StockQuoteAlert
{
    public class StockQuoteMonitor : IObserver<StockInfo>
    {
        private readonly ILogger<StockQuoteMonitor> _log;
        private readonly IStockQuoteObservable _stockObservable;
        public StockQuoteMonitor(ILogger<StockQuoteMonitor> log, IStockQuoteObservable stockObservable)
        {
            _log = log;
            _stockObservable = stockObservable;
        }

        public void OnCompleted()
        {
            throw new InvalidOperationException("Esse observador não deveria ser terminado.");
        }

        public void OnError(Exception error)
        {
            _log.LogError(error, "Erro durante o processamento.");
        }

        public void OnNext(StockInfo value)
        {
            // Lógica do monitoramento
        }

        public void StartMonitoring()
        {
            _log.LogInformation("Iniciando monitoramento...");
            _stockObservable.Subscribe(this);
        }
    }
}
