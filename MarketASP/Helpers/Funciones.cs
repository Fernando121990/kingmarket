using MarketASP.Models;
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
        public static string ObtenerValorParam(string categoria, string nombre)
        {
            MarketWebEntities db = new MarketWebEntities();
            var valor = db.CONFIGURACION.Where(p => p.stipo_confi == categoria && p.sdesc_confi == nombre).SingleOrDefault().svalor_confi;
            return valor;
        }
        public static string ObtenerValorParam(string categoria, int codigo)
        {
            MarketWebEntities db = new MarketWebEntities();
            var valor = db.CONFIGURACION.Where(p => p.stipo_confi == categoria && p.ncode_confi == codigo).SingleOrDefault().svalor_confi;
            return valor;
        }

    }
}