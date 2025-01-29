using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarketASP.Models;
using Newtonsoft.Json;

namespace MarketASP.Clases
{
    public class proformaView : PROFORMAS
    {
        public string sfeprofo_prof { get; set; }
        public string sfevenci_prof { get; set; }
        [JsonProperty(PropertyName = "proformaViewDetas")]
        public IList<proformaViewDeta> proformaViewDetas { get; set; }

    }
}