using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class guiaView :GUIA
    {

        public string sfemov_guia { get; set; }            

        [JsonProperty(PropertyName = "guiaViewDetas")]
        public IList<guiaViewDeta> guiaViewDetas { get; set; }

        [JsonProperty(PropertyName = "guiaViewLotes")]
        public IList<guiaViewLote> guiaViewLotes { get; set; }

        [JsonProperty(PropertyName = "guiaViewCuotas")]
        public IList<guiaViewCuota> guiaViewCuotas { get; set; }

    }
}