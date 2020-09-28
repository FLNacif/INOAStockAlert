using INOA.Challenge.StockApi.Mock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INOA.Challenge.StockApi.Mock.IService
{
    public interface IStockQuoteService
    {
        public Dictionary<string, StockDataResponseDto> BuscarCotacao(IList<string> symbols);
    }
}
