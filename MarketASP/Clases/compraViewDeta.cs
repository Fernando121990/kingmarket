using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class compraViewDeta:COMPRA_DETALLE
    {
        public bool? bisc_comdeta { get; set; }
        public string scod2 { get; set; }
        public string sdesc { get; set; }
        public string sund { get; set; }
    }
}