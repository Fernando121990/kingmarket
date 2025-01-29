using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class permisoView : Permiso
    {
        public List<ventanaView> detalle { get; set; }
    }
}