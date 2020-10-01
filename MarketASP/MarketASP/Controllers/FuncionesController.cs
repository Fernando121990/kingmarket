using MarketASP.Extensiones;
using MarketASP.Clases;
using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
            var result = db.Pr_ClienteBusca(1, sdescCliente, "", "").ToList();
            return Json(result);
        }

        public JsonResult getRucCliente(string srucCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_ClienteBusca(1, "",srucCliente, "").ToList();
            return Json(result);
        }

        public JsonResult getDniCliente(string sdniCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_ClienteBusca(1, "", "",sdniCliente).ToList();
            return Json(result);
        }

        public JsonResult getDocuNumero(int ndocu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_DocNumeracion(1,User.Identity.Name, int.Parse(User.Identity.GetLocal()),ndocu).ToList();
            return Json(result);
        }

        public JsonResult getDiasFormaPago(int ncode_fopago)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.CONFIGURACION
                         where (s.ncode_confi == ncode_fopago)
                         select new { dias = s.svalor_confi };
            return Json(result);
        }

        public JsonResult getProveedor(string sdescProvee)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.PROVEEDOR
                         where s.sdesc_prove.Contains(sdescProvee)
                         select new { s.ncode_provee, s.sdesc_prove};
            return Json(result);
        }

        public JsonResult getClienteDire(Int32 scodCliente)
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
        public decimal fnnumeroeditar(string id, decimal value, int column)
        {
            return value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateCliente(string model_json)
        {
            ObjectParameter xcod = new ObjectParameter("xcod", typeof(String));
            ObjectParameter ingproc_001 = new ObjectParameter("ingproc_001", typeof(long));
            string code;
            string data = "";
            try
            {

                if (ModelState.IsValid)
                {
                    data = model_json;

                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    if (data != null)
                    {
                        var mofView = JsonConvert.DeserializeObject<clienteView>(data, jsonSettings);

                        if (mofView != null)
                        {
                            //VERIFICAR RUC O DNI
                            clienteView cLIENTE = mofView;

                            if (cLIENTE.stipo_cliente == "J")
                            {
                                CLIENTE xcli = db.CLIENTE.SingleOrDefault(c => c.sruc_cliente == cLIENTE.sruc_cliente);
                                if (xcli != null)
                                {
                                    ViewBag.mensaje = "El cliente ya existe";
                                    //return View("_Mensaje");
                                    return Json(new { Success = 0 });
                                }

                            }

                            if (cLIENTE.stipo_cliente == "N")
                            {
                                CLIENTE xcli = db.CLIENTE.SingleOrDefault(c => c.sdnice_cliente == cLIENTE.sdnice_cliente);
                                if (xcli != null)
                                {
                                    ViewBag.mensaje = "El cliente ya existe";
                                    //return View("_Mensaje");
                                    return Json(new { Success = 0 });
                                }

                            }

                            CLIENTE cliente = ToCliente(cLIENTE);

                            db.CLIENTE.Add(cliente);
                            db.SaveChangesAsync();

                            int id = cliente.ncode_cliente;
                            CLI_DIRE cLI_DIRE = toCliDire(cLIENTE, id);

                            db.CLI_DIRE.Add(cLI_DIRE);
                            db.SaveChangesAsync();

                            ///return RedirectToAction("Index");

                            //db.MANT_CLIENTES("", mofView.srazon_cliente, mofView.sruc_cliente, mofView.sdni_cliente, mofView.sfono_cliente, "", 0, mofView.srep_cliente
                            //    , mofView.semail_cliente, "", true, "", mofView.sfono2_cliente, "", "", 0, "W", "", "", "", "", "", 0, mofView.ape_pat_cliente,
                            //    mofView.ape_mat_cliente, mofView.nombres_cliente, mofView.tipo_contribuyente, ingproc_001, xcod);

                            //code = xcod.Value.ToString();

                            //db.GRABAR_DIR_CLIENTE(code, mofView.sdire_cliente, mofView.subigeo_cliente, "", 0, ingproc_001);

                        }
                    }
                }

                return Json(new { Success = 1 });

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                ViewBag.mensaje = mensaje;
                return Json(new { Success = 0 });
            }
        }

        private CLI_DIRE toCliDire(clienteView cLIENTE, int id)
        {
            return new CLI_DIRE
            {
                scode_ubigeo = cLIENTE.scode_ubigeo,
                sdesc_clidire = cLIENTE.sdire_cliente,
                ncode_cliente = id
            };
        }

        private CLIENTE ToCliente(clienteView cliView)
        {
            return new CLIENTE
            {
                bprocedencia_cliente = cliView.bprocedencia_cliente,
                dfech_cliente = DateTime.Now,
                sapma_cliente = cliView.sapma_cliente,
                sappa_cliente = cliView.sappa_cliente,
                sdnice_cliente = cliView.sdnice_cliente,
                sfax_cliente = cliView.sfax_cliente,
                sfono1_cliente = cliView.sfono1_cliente,
                sfono2_cliente = cliView.sfono2_cliente,
                sfono3_cliente = cliView.sfono3_cliente,
                slineacred_cliente = cliView.slineacred_cliente,
                smail_cliente = cliView.smail_cliente,
                snomb_cliente = cliView.snomb_cliente,
                sobse_cliente = cliView.sobse_cliente,
                srazon_cliente = (cliView.stipo_cliente == "J") ? cliView.srazon_cliente : cliView.snomb_cliente + " " + cliView.sappa_cliente + " " + cliView.sapma_cliente,
                srepre_cliente = cliView.srepre_cliente,
                sruc_cliente = cliView.sruc_cliente,
                stipo_cliente = cliView.stipo_cliente,
                suser_cliente = User.Identity.Name,
                sweb_cliente = cliView.sweb_cliente,
            };

        }
        //private ActionResult RedirectToTipoCambio(string returnUrl)
        //{

        //    MarketWebEntities db = new MarketWebEntities();

        //    ObjectParameter valor = new ObjectParameter("valor", typeof(int));
        //    int xvalor = 0;
        //    var result = db.Pr_tipoCambioExiste(DateTime.Today.ToShortDateString(), valor);
        //    xvalor = int.Parse(valor.Value.ToString());

        //    if (xvalor == 0)
        //    {
        //        return RedirectToAction("Create", "tipo_cambio");
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
    }
}