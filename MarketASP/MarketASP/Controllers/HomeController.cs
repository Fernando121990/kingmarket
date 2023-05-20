using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
   
    public class HomeController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();
        public ActionResult Index()
        {

            ViewBag.ruta = ControllerContext.HttpContext.Server.MapPath("/");
            string RutaAplicacion = ControllerContext.HttpContext.Server.MapPath("/");
            string RutaCertificado = Helpers.Funciones.ObtenerValorParam("RUTA", "CERTIFICADO");
            string RutaXML = Helpers.Funciones.ObtenerValorParam("RUTA", "XML");
            string RutaCDR = Helpers.Funciones.ObtenerValorParam("RUTA", "CDR");
            string RutaEnvio = Helpers.Funciones.ObtenerValorParam("RUTA", "ENVIO");
            string RutaQR = Helpers.Funciones.ObtenerValorParam("RUTA", "QR");
            //Enviar las credenciales
            ViewBag.certificado = Path.Combine(RutaAplicacion, RutaCertificado);
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}