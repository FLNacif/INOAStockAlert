using INOA.Challenge.Entities.Configuration;
using INOA.Challenge.IStockObservable;
using INOA.Challenge.StockObservable.StockQuoteApiObservable;
using INOA.Challenge.StockQuoteAlert.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;

namespace INOA.Challenge.StockQuoteAlert
{
    class Program
    {
        private static EventWaitHandle WaitBackground = new EventWaitHandle(false, EventResetMode.ManualReset);
        static void Main(string[] args)
        {
            // Configurando DI
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var stockCode = args[0];
            double sellPrice;
            if (!Double.TryParse(args[1].Replace('.', ','), out sellPrice))
            {
                Console.WriteLine("O segundo argumento (preço de venda) deve ser numérico.");
            }
            double buyPrice;
            if (!Double.TryParse(args[2].Replace('.', ','), out buyPrice))
            {
                Console.WriteLine("O terceiro argumento (preço de compra) deve ser numérico.");
            }

            var stockQuoteMonitor = serviceProvider.GetService<StockQuoteMonitor>();
            stockQuoteMonitor.StartMonitoring(stockCode, sellPrice, buyPrice);

            // Mantem o console rodando, até que a aplicação seja finalizada
            WaitBackground.WaitOne();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddLogging(configure => configure.AddConsole())
                .AddSingleton<IStockQuoteObservable, StockQuoteApiObservable>()
                .AddSingleton(SetupConfiguration())
                .AddTransient<IStockQuoteApiService, StockQuoteApiService>()
                .AddTransient<INotificationService, NotificationService>()
                .AddTransient<StockQuoteMonitor>();
        }

        private static ProgramConfiguration SetupConfiguration()
        {
            ProgramConfiguration config = new ProgramConfiguration();
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .Bind(config);

            return config;
        }
    }
}
