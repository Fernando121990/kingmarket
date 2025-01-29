using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class ventaViewDeta : VENTA_DETALLE
    {
        public bool? bisc_vedeta { get; set; }
        public bool? blote_vedeta { get; set; }
        public bool? bdescarga_vedeta { get; set; }
        public string scod2 { get; set; }
        public string sdesc { get; set; }
        
        public string sumed { get; set; }
        public long? ncode_umed { get; set; }
    }
}