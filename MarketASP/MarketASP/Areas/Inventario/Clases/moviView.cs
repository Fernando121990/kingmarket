using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketASP.Areas.Inventario.Clases
{
    public class moviView 
    {
        public int ncode_movi { get; set; }
        public string dfemov_movi { get; set; }
        public string smone_movi { get; set; }
        public decimal ntc_movi { get; set; }
        public string sobse_movi { get; set; }
        public int ncode_timovi { get; set; }
        public int ncode_alma { get; set; }
        public int? ndestino_alma { get; set; }
        public string stipo_movi { get; set; }
        [JsonProperty(PropertyName = "moviViewDetas")]
        public IList<moviViewDeta> moviViewDetas { get; set; }
    }

}
