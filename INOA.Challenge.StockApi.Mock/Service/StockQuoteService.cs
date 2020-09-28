using CsvHelper;
using INOA.Challenge.StockApi.Mock.Entities;
using INOA.Challenge.StockApi.Mock.IService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace INOA.Challenge.StockApi.Mock.Service
{
    public class StockQuoteService : IStockQuoteService
    {
        private IEnumerable<StockCsvEntity> _dadosAcoesB3;
        private Dictionary<string, StockDataResponseDto> _ultimaResposta;
        private DateTime _dataAtualDado;
        public StockQuoteService()
        {
            using (var reader = new StreamReader(".\\Resources\\2020.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                _dadosAcoesB3 = csv.GetRecords<StockCsvEntity>().ToList();
            }
            _dataAtualDado = new DateTime(2020, 1, 1);
            _ultimaResposta = new Dictionary<string, StockDataResponseDto>();
        }

        public Dictionary<string, StockDataResponseDto> BuscarCotacao(IList<string> symbols)
        {
            var resultado = new Dictionary<string, StockDataResponseDto>();
            IEnumerable<StockCsvEntity> dadosFiltrados;
            do
            {
                if (_dataAtualDado.Year > 2020)
                {
                    _dataAtualDado = new DateTime(22020, 1, 1);
                }
                dadosFiltrados = _dadosAcoesB3.Where(x => x.Data == _dataAtualDado && symbols.Contains(x.Ticker));
                _dataAtualDado = _dataAtualDado.AddDays(1);
            } while (!dadosFiltrados.Any()); // para pular finais de semana ou datas em que aquele papel não existia, por exemplo;

            foreach (var dado in dadosFiltrados)
            {
                var stockData = new StockDataResponseDto()
                {
                    currency = "BRL",
                    market_time = new MarketTimeResponseDto(),
                    updated_at = dado.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                    symbol = dado.Ticker,
                    name = dado.Ticker,
                    region = "Brazil",
                    price = dado.PrecoMedio,
                    market_cap = 0
                };
                if (_ultimaResposta.ContainsKey(dado.Ticker))
                {
                    stockData.change_percent = ((stockData.price / _ultimaResposta[dado.Ticker].price) - 1) * 100;
                }

                resultado.Add(dado.Ticker, stockData);
            }
            _ultimaResposta = resultado;
            return resultado;
        }
    }
}
