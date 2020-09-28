using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INOA.Challenge.StockApi.Mock.Entities
{
    public class StockCsvEntity
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }

        public DateTime Data => new DateTime(Ano, Mes, Dia);
        public string TipoMercado { get; set; }
        public string Ticker { get; set; }
        public double PrecoAbertura { get; set; }
        public double PrecoMax { get; set; }
        public double PrecoMin { get; set; }
        public double PrecoMedio { get; set; }
        public double PrecoFechamento { get; set; }
        public int NumeroNegocios { get; set; }
        public double VolumeNegociado { get; set; }

    }
}
