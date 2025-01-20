using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class clienteView : CLIENTE
    {
        public string scode_ubigeo { get; set; }
        public string sdire_cliente { get; set; }
    }
}