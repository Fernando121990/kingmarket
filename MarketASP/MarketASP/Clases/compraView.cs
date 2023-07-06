using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class compraView:COMPRAS
    {
        public string sproveedor { get; set; }
        public string sruc { get; set; }
        public string sfecompra_compra { get; set; }
        public string sfevenci_compra { get; set; }
        [JsonProperty(PropertyName = "compraViewDetas")]
        public IList<compraViewDeta> compraViewDetas { get; set; }
        [JsonProperty(PropertyName = "loteViewDeta")]
        public IList<loteViewDeta> loteViewDeta { get; set; }

    }
}