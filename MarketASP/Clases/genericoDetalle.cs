using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketASP.Clases
{
    public class genericoDetalle
    {
        public long ncodigo { get; set; }
        public string sdescripcion { get; set; }
        public string scodigo { get; set; }
        public string sumedida { get; set; }
        public long? numedida { get; set; }
        public decimal? ncantidad { get; set; }
        public decimal? nprecio { get; set; }
        public int? nalmacen { get; set;}

    }
}
