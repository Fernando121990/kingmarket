using MarketASP.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarketASP.Clases
{
    public class ventaView : VENTAS
    {
        public string sserienumero { get; set; }
        public string sfeventa_venta { get; set; }
        public string sfevenci_venta { get; set; }
        public string scliente { get; set; }
        public string sruc { get; set; }
        public string sdni { get; set; }
        [JsonProperty(PropertyName = "ventaViewDetas")]
        public IList<ventaViewDeta> ventaViewDetas { get; set; }
        
        [JsonProperty(PropertyName = "ventaViewLotes")]
        public IList<ventaViewLote> ventaViewLotes { get; set; }

    }
}