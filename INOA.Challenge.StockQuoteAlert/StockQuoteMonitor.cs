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
        private StockInfo _lastValue;
        private double _sellPrice;
        private double _buyPrice;
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
            _log.LogDebug($"Recebida uma atualização de preço doa tivo {value.StockCode} com o valor de R${value.StockPrice}.");
            // Lógica do monitoramento
            if (_lastValue.StockPrice >= _buyPrice && value.StockPrice < _buyPrice)
            {
                // Dispara e-mail de compra
                _log.LogDebug($"Preço de compra atingido, disparando um e-mail.");
                return;
            }
            if (_lastValue.StockPrice <= _sellPrice && value.StockPrice > _sellPrice)
            {
                // Dispara e-mail de venda
                _log.LogDebug($"Preço de venda atingido, disparando um e-mail.");
                return;
            }

            // Atualiza o último preço.
            _lastValue = value;
        }

        public void StartMonitoring(string code, double sellPrice, double buyPrice)
        {
            _log.LogInformation($"Iniciando monitoramento do código {code} com preço de venda em R${sellPrice} e preço de compra em R${buyPrice}");
            _sellPrice = sellPrice;
            _buyPrice = buyPrice;
            _stockObservable.Subscribe(this, code);
        }
    }
}
