using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Helpers
{
    public class Funciones
    {
        public decimal FnRedondear(decimal? valor,int num) {

            decimal xvalor = valor.GetValueOrDefault(0);

            return  decimal.Round(xvalor, num);

        }
    }
}