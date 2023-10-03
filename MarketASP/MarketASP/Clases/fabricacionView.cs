using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketASP.Clases
{
    public class fabricacionView:Fabricacion
    {

        public string sfab_fecha { get; set; }
        public string sfab_fvenc { get; set; } 

        [JsonProperty(PropertyName = "fabricacionViewDetas")]
        public IList<fabricacionViewDeta> fabricacionViewDetas { get; set; }
    }
}
