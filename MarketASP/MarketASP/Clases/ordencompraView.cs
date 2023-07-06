using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class ordencompraView:ORDEN_COMPRAS
    {
        public string sfeordencompra_orco { get; set; }
        public string sfevenci_orco { get; set; }
        public string sfentrega_orco { get; set; }


        [JsonProperty(PropertyName = "ordencompraViewDetas")]
        public IList<ordencompraViewDeta> ordencompraViewDetas { get; set; }
    }
}