using INOA.Challenge.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace INOA.Challenge.StockObservable.StockQuoteApiObservable
{
    public interface IStockQuoteApiService
    {
        Task<IList<StockInfo>> BuscarCotacao(IEnumerable<string> codes = null);
    }
}
