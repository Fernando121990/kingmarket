using MarketASP.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarketASP.Clases
{
    public class ventaView : VENTAS
    {
        public string sfeventa_venta { get; set; }
        public string sfevenci_venta { get; set; }
        [JsonProperty(PropertyName = "ventaViewDetas")]
        public IList<ventaViewDeta> ventaViewDetas { get; set; }

    }
}