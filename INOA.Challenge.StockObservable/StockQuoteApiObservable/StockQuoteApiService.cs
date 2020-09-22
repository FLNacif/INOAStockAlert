using INOA.Challenge.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace INOA.Challenge.StockObservable.StockQuoteApiObservable
{
    public class StockQuoteApiService : IStockQuoteApiService
    {

        public StockQuoteApiService()
        {

        }

        public async Task<IList<StockInfo>> BuscarCotacao(IEnumerable<string> codes = null)
        {
            return await Task.FromResult(new List<StockInfo>() { new StockInfo() {StockCode = "PETR4", StockPrice = 12.12 } });
        }
    }
}
