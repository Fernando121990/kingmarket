using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    public class FuncionesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public JsonResult getUbigeo(string sdescUbigeo)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.UBIGEO
                         where (s.sdistri_ubigeo + s.sprovi_ubigeo + s.sdepa_ubigeo).Contains(sdescUbigeo)
                         select new { codigo = s.scode_ubigeo, direccion = s.sdistri_ubigeo + " " + s.sprovi_ubigeo + " " + s.sdepa_ubigeo };
            return Json(result);
        }


        public JsonResult getCliente(string sdescCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.CLIENTE
                         where s.srazon_cliente.Contains(sdescCliente)
                         select new { s.ncode_cliente, s.srazon_cliente };
            return Json(result);
        }

        public JsonResult getClienteDire(string scodCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.CLI_DIRE
                         where s.ncode_cliente.Equals(scodCliente)
                         select new { s.ncode_clidire, s.sdesc_clidire };
            return Json(result);
        }

        public JsonResult getArticulos()
        {
            var result = db.Pr_ConsultaArticulos();

            return Json(result); 
        }

        //public async Task<JsonResult> getArticulos()
        //{
        //    var result = db.Pr_ConsultaArticulos();

        //    return Json(await Task.FromResult(result));
        //}

        //public JsonResult getArticulos(int listaPrecio)
        //{
        //    db.Configuration.ProxyCreationEnabled = false;
        //    var result = from s in db.ARTICULO
        //                 join m in db.UMEDIDA
        //                 on s.ncode_umed equals m.ncode_umed
        //                 join p in db.ART_PRECIO
        //                 on s.ncode_arti equals p.ncode_arti
        //                 where s.nesta_arti == true && p.ncode_lipre == listaPrecio
        //                 select new
        //                 {
        //                     codigo = s.ncode_arti,
        //                     afecto = s.bafecto_arti,
        //                     isc = s.bisc_arti,
        //                     scodigo = s.scode_arti,
        //                     descripcion = s.sdesc1_arti,
        //                     medida = m.sdesc_umed,
        //                     stock = Math.Round(s.nstockmin_arti ?? 0, 2),
        //                     precio = p.nprecio_artpre,
        //                     moneda = ""
        //                 };
        //    return Json(result);
        //}
        public string fncadenaeditar(string id, string value, int column)
        {
            return value;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public JsonResult CreateCliente(string model_json)
        //{
        //    ObjectParameter xcod = new ObjectParameter("xcod", typeof(String));
        //    ObjectParameter ingproc_001 = new ObjectParameter("ingproc_001", typeof(long));
        //    string code;
        //    string data = "";
        //    try
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            data = model_json;

        //            var jsonSettings = new JsonSerializerSettings
        //            {
        //                NullValueHandling = NullValueHandling.Ignore
        //            };
        //            if (data != null)
        //            {
        //                var mofView = JsonConvert.DeserializeObject<clienteView>(data, jsonSettings);

        //                if (mofView != null)
        //                {


        //                    db.MANT_CLIENTES("", mofView.srazon_cliente, mofView.sruc_cliente, mofView.sdni_cliente, mofView.sfono_cliente, "", 0, mofView.srep_cliente
        //                        , mofView.semail_cliente, "", true, "", mofView.sfono2_cliente, "", "", 0, "W", "", "", "", "", "", 0, mofView.ape_pat_cliente,
        //                        mofView.ape_mat_cliente, mofView.nombres_cliente, mofView.tipo_contribuyente, ingproc_001, xcod);

        //                    code = xcod.Value.ToString();

        //                    db.GRABAR_DIR_CLIENTE(code, mofView.sdire_cliente, mofView.subigeo_cliente, "", 0, ingproc_001);

        //                }
        //            }
        //        }

        //        return Json(new { Success = 1 });

        //    }
        //    catch (Exception ex)
        //    {
        //        string mensaje = ex.Message;
        //        ViewBag.mensaje = mensaje;
        //        return Json(new { Success = 0 });
        //    }
        //}
    }
}