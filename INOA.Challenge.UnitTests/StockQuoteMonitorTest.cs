using INOA.Challenge.Entities;
using INOA.Challenge.IStockObservable;
using INOA.Challenge.StockQuoteAlert;
using INOA.Challenge.StockQuoteAlert.Notification;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace INOA.Challenge.UnitTests
{
    [TestClass]
    public class StockQuoteMonitorTest
    {
        private readonly Mock<ILogger<StockQuoteMonitor>> _log;
        private readonly Mock<IStockQuoteObservable> _stockObservable;
        private readonly Mock<INotificationService> _notificationService;

        private StockQuoteMonitor _stockQuoteMonitor;


        public StockQuoteMonitorTest()
        {
            _log = new Mock<ILogger<StockQuoteMonitor>>();
            _stockObservable = new Mock<IStockQuoteObservable>();
            _notificationService = new Mock<INotificationService>();

        }

        [TestMethod]
        public void TestarPrecoInicialAcimaVenda_DisparaEmailVenda()
        {
            _stockQuoteMonitor = new StockQuoteMonitor(_log.Object, _stockObservable.Object, _notificationService.Object);

            _stockQuoteMonitor.StartMonitoring("PETR4", 20, 10);

            var listaCotacoes = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 30,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(listaCotacoes);

            _notificationService.Verify(x => x.Notify(ApplicationConstants.SubjectVenda, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TestarPrecoInicialAbaixoCompra_DisparaEmailCompra()
        {
            _stockQuoteMonitor = new StockQuoteMonitor(_log.Object, _stockObservable.Object, _notificationService.Object);

            _stockQuoteMonitor.StartMonitoring("PETR4", 20, 10);

            var listaCotacoes = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 5,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(listaCotacoes);

            _notificationService.Verify(x => x.Notify(ApplicationConstants.SubjectCompra, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TestarPrecoInicialIntervalo_NaoDisparaEmail()
        {
            _stockQuoteMonitor = new StockQuoteMonitor(_log.Object, _stockObservable.Object, _notificationService.Object);

            _stockQuoteMonitor.StartMonitoring("PETR4", 20, 10);

            var listaCotacoes = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 15,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(listaCotacoes);

            _notificationService.Verify(x => x.Notify(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void TestarPrecoInicialIntervaloSeguidoDeSubida_DisparaEmailVenda()
        {
            _stockQuoteMonitor = new StockQuoteMonitor(_log.Object, _stockObservable.Object, _notificationService.Object);

            _stockQuoteMonitor.StartMonitoring("PETR4", 20, 10);

            var primeiraCotacao = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 15,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(primeiraCotacao);

            var segundaCotacao = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 30,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(segundaCotacao);

            _notificationService.Verify(x => x.Notify(ApplicationConstants.SubjectVenda, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void TestarPrecoInicialIntervaloSeguidoDeDescida_DisparaEmailCompra()
        {
            _stockQuoteMonitor = new StockQuoteMonitor(_log.Object, _stockObservable.Object, _notificationService.Object);

            _stockQuoteMonitor.StartMonitoring("PETR4", 20, 10);

            var primeiraCotacao = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 15,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(primeiraCotacao);

            var segundaCotacao = new List<StockInfo>() { new StockInfo() {
                StockCode = "PETR4",
                StockPrice = 5,
                Timestamp = DateTime.Now
                }
            };

            _stockQuoteMonitor.OnNext(segundaCotacao);

            _notificationService.Verify(x => x.Notify(ApplicationConstants.SubjectCompra, It.IsAny<string>()), Times.Once);
        }
    }
}
