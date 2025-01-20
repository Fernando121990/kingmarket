using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class loteView
    {
        public int ncode_compra { get; set; }
        [JsonProperty(PropertyName = "loteViewDeta")]
        public IList<loteViewDeta> loteViewDeta { get; set; }
    }
}