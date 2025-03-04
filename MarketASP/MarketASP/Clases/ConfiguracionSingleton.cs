﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MarketASP.Clases
{
    public class ConfiguracionSingleton
    {
        public static bool confiCambio = false;
        private static ConfiguracionSingleton instance = null;
        public decimal glbIGV { get; set; }
        public string glbcobroAutomatico { get; set; }
        public int glbDecimales { get; set; }
        public string glbVentaxPedido { get; set; }
        public int glbContado { get; set; }
        public ConfiguracionSingleton()
        {
            using (Models.MarketWebEntities db = new Models.MarketWebEntities())
            {
                var lblconfiguracion = db.CONFIGURACION.Where(c => c.ntipo_confi == 1).ToList();
                glbIGV = decimal.Parse(lblconfiguracion.Where(c => c.ncode_confi == 1).First().svalor_confi);
                glbcobroAutomatico = lblconfiguracion.Where(c => c.ncode_confi == 1012).First().svalor_confi;
                glbDecimales = int.Parse(lblconfiguracion.Where(c => c.ncode_confi == 1031).First().svalor_confi);
                glbVentaxPedido = lblconfiguracion.Where(c => c.ncode_confi == 1068).First().svalor_confi;
                glbContado = 1053;
            }

        }
        public static ConfiguracionSingleton Instance
        {
            get
            {
                if (instance == null || confiCambio)
                {
                    instance = new ConfiguracionSingleton();
                }
                return instance;
            }
        }
    }
}