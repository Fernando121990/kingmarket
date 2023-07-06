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
        [JsonProperty(PropertyName = "guiaViewDetas")]
        public IList<guiaViewDeta> guiaViewDetas { get; set; }
    }
}