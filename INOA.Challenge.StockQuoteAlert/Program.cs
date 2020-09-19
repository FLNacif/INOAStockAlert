using INOA.Challenge.IStockObservable;
using INOA.Challenge.StockObservable;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Authentication.ExtendedProtection;

namespace INOA.Challenge.StockQuoteAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configurando DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var stockQuoteMonitor = serviceProvider.GetService<StockQuoteMonitor>();
            stockQuoteMonitor.StartMonitoring();

        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                .AddSingleton<IStockQuoteObservable, StockQuoteApiObservable>()
                .AddTransient<StockQuoteMonitor>();
        }
    }
}
