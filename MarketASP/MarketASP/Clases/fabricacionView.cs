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
        [JsonProperty(PropertyName = "fabricacionViewDetas")]
        public IList<fabricacionViewDeta> fabricacionViewDetas { get; set; }
    }
}
