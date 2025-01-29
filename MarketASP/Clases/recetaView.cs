using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketASP.Clases
{
    public class recetaView : Receta
    {
        [JsonProperty(PropertyName = "recetaViewDetas")]
        public IList<recetaViewDeta> recetaViewDetas { get; set; }
    }
}
