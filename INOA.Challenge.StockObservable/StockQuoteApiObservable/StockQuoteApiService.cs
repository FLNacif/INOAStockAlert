using INOA.Challenge.Entities;
using INOA.Challenge.Entities.Configuration;
using INOA.Challenge.StockObservable.StockQuoteApiObservable.Dto;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace INOA.Challenge.StockObservable.StockQuoteApiObservable
{
    public class StockQuoteApiService : IStockQuoteApiService
    {

        private static HttpClient _http = new HttpClient();
        private readonly ProgramConfiguration _config;

        public StockQuoteApiService(ProgramConfiguration config)
        {
            _config = config;
        }

        public async Task<IList<StockInfo>> BuscarCotacao(IEnumerable<string> codes = null)
        {
            var request = await _http.GetAsync($"{_config.Api.BaseUrl}?key={_config.Api.Key}&symbol={string.Join(",", codes)}");

            if (request.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var contentJson = await request.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<StockResponseDto>(contentJson);

                return content.results.Select(x => new StockInfo()
                {
                    StockCode = x.Key,
                    StockPrice = x.Value.price,
                    Timestamp = DateTime.ParseExact(x.Value.updated_at, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                }).ToList();
            }
            return new List<StockInfo>();
        }
    }
}
