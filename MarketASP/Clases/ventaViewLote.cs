using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class ventaViewLote : VENTA_LOTE
    {
        public string scod2 { get; set; }
        public string sdesc { get; set; }
        public string sfvenci_lote { get; set; }     
    }
}