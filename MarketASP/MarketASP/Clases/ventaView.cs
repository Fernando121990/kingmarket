using MarketASP.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarketASP.Clases
{
    public class ventaView : VENTAS
    {
        [JsonProperty(PropertyName = "ventaViewDetas")]
        public IList<ventaViewDeta> ventaViewDetas { get; set; }

    }
}