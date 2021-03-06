﻿using INOA.Challenge.Entities;
using INOA.Challenge.IStockObservable;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using INOA.Challenge.StockQuoteAlert.Notification;

namespace INOA.Challenge.StockQuoteAlert
{
    public class StockQuoteMonitor : IObserver<IEnumerable<StockInfo>>
    {
        private readonly ILogger<StockQuoteMonitor> _log;
        private readonly IStockQuoteObservable _stockObservable;
        private readonly INotificationService _notificationService;
        private IDictionary<string, StockInfo> _lastValues;
        private double _sellPrice;
        private double _buyPrice;
        public StockQuoteMonitor(ILogger<StockQuoteMonitor> log, IStockQuoteObservable stockObservable, INotificationService notificationService)
        {
            _log = log;
            _stockObservable = stockObservable;
            _lastValues = new Dictionary<string, StockInfo>();
            _notificationService = notificationService;
        }

        public void OnCompleted()
        {
            throw new InvalidOperationException("Esse observador não deveria ser terminado.");
        }

        public void OnError(Exception error)
        {
            _log.LogError(error, "Erro durante o processamento.");
        }

        public void OnNext(IEnumerable<StockInfo> quotes)
        {
            foreach (var quote in quotes)
            {
                StockInfo _lastQuote;
                _lastValues.TryGetValue(quote.StockCode, out _lastQuote);
                _log.LogInformation($"Recebida uma atualização de preço do ativo {quote.StockCode} com o valor de R${quote.StockPrice}.");
                // Lógica do monitoramento
                if ((_lastQuote == null || _lastQuote.StockPrice >= _buyPrice) && quote.StockPrice < _buyPrice)
                {
                    // Dispara e-mail de compra
                    _log.LogInformation($"Preço de compra atingido, disparando um e-mail.");
                    NotificarCompra(quote);
                }
                else if ((_lastQuote == null || _lastQuote.StockPrice <= _sellPrice) && quote.StockPrice > _sellPrice)
                {
                    // Dispara e-mail de venda
                    _log.LogInformation($"Preço de venda atingido, disparando um e-mail.");
                    NotificarVenda(quote);
                }

                // Atualiza o último preço.
                _lastValues[quote.StockCode] = quote;
            }
        }

        private void NotificarVenda(StockInfo stock)
        {
            _notificationService.Notify(ApplicationConstants.SubjectVenda, $"O preço de venda para o papel {stock.StockCode} foi atingido. Preço atual: R${stock.StockPrice}. ");
        }

        private void NotificarCompra(StockInfo stock)
        {
            _notificationService.Notify(ApplicationConstants.SubjectCompra, $"O preço de compra para o papel {stock.StockCode} foi atingido. Preço atual: R${stock.StockPrice}. ");
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