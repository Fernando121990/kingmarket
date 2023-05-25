using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketASP.Models;
using Newtonsoft.Json;

namespace MarketASP.Clases
{
    public class ordenpedidoView : ORDEN_PEDIDOS
    {
        public string sfeordenpedido_orpe { get; set; }
        public string sfevenci_orpe { get; set; }
        [JsonProperty(PropertyName = "ordenpedidoViewDetas")]
        public IList<ordenpedidoViewDeta> ordenpedidoViewDetas { get; set; }

    }
}