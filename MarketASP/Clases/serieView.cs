using MarketASP.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarketASP.Clases
{
    public class serieView : DOCU_SERIE
    {
        public string[] Id { get; set; }

        public List<serieViewDeta> uservalues { get; set; }

    }
}