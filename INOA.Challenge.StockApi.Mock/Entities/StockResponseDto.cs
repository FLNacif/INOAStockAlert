using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INOA.Challenge.StockApi.Mock.Entities
{
    public class StockResponseDto
    {
        public string by { get; set; }
        public bool valid_key { get; set; }
        public Dictionary<string, StockDataResponseDto> results { get; set; }
        public double execution_time { get; set; }
        public bool from_cache { get; set; }
    }
}
