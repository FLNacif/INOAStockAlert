using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INOA.Challenge.StockApi.Mock.Entities
{
    public class MarketTimeResponseDto
    {
        public MarketTimeResponseDto()
        {
            open = "10:00";
            close = "17:30";
            timezone = -3;
        }
        public string open { get; set; }
        public string close { get; set; }
        public int timezone { get; set; }
    }
}
