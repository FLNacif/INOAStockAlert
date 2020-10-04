using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using INOA.Challenge.StockApi.Mock.Entities;
using INOA.Challenge.StockApi.Mock.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace INOA.Challenge.StockApi.Mock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockQuoteController : ControllerBase
    {
        private readonly ILogger<StockQuoteController> _logger;
        private readonly IStockQuoteService _stockQuoteService;

        public StockQuoteController(ILogger<StockQuoteController> logger, IStockQuoteService stockQuoteService)
        {
            _logger = logger;
            _stockQuoteService = stockQuoteService;
        }

        // Está API imita o contrato fornecido por: https://console.hgbrasil.com/documentation/finance#obtendo-preco-de-acoes
        // A intenção é que se possa fazer testes sem que precise de fato utilizar a API da HGBrasil que possui limites de requisições.
        // Será utilizado como fonte dos dados um arquivo de dados diários da B3 em um formato que eu já possuía em CSV de um outro projeto.

        [HttpGet]
        public StockResponseDto Get(string symbol)
        {
            symbol = symbol ?? "";
            var symbols = symbol.Split(",");
            var response = new StockResponseDto();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            response.results = _stockQuoteService.BuscarCotacao(symbols);

            sw.Stop();
            response.execution_time = sw.ElapsedMilliseconds / 1000.0;
            response.from_cache = false;
            response.valid_key = true;
            response.by = "symbol";

            return response;
        }
    }
}
