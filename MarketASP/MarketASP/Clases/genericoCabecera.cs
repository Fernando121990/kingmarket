using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketASP.Clases
{
    public class genericoCabecera
    {
        public long codigo { get; set; }
        public decimal? cantidad { get; set; }
        public decimal? costoOperativo { get; set; }

        [JsonProperty(PropertyName = "detalle")]
        public IList<genericoDetalle> detalle { get; set; }

    }
}
