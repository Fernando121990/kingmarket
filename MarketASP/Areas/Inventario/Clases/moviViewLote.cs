using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketASP.Areas.Inventario.Clases
{
    public class moviViewLote : MOVI_LOTE
    {
        public string scod2 { get; set; }
        public string sdesc { get; set; }
        public string sfvenci_lote { get; set; }
    }
}
