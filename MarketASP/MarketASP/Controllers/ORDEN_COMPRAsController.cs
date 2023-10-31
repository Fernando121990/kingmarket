using MarketASP.Clases;
using MarketASP.Extensiones;
using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    [Authorize]
    public class ORDEN_COMPRAsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

  
        public ActionResult Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0401", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var ORDEN_COMPRAS = db.Pr_OCompraConsulta(1, 0,"","").ToList();
            return View(ORDEN_COMPRAS);
        }

        public async Task<ActionResult> Details(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0406", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDEN_COMPRAS pedido = await db.ORDEN_COMPRAS.FindAsync(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        public async Task<ActionResult> OPDetalles()
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var result = db.Pr_OCompraConsultaDetallada(0).ToList();

            return View(result);
        }

        // GET: ORDEN_COMPRAS/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0402", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                ViewBag.mensaje = "No se ha registrado el tipo de cambio, comuniquese con el administrador del sistema";
                return View("_Mensaje");
                //                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;
            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";
            
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).
                Where(c => c.ntipo_confi == 5 && c.svalor_confi == "O"), "ncode_confi", "sdesc_confi",1074);
            ViewBag.sseri_orco = new SelectList(db.Pr_DocSerie(1, User.Identity.Name, 0, 1074), "ncode_dose", "serie");
            ViewBag.smone_orco = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ViewBag.moneda);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.dfeorco_orco = string.Format("{0:dd/MM/yyyy}", yfecha);
            ViewBag.dfevenci_orco = string.Format("{0:dd/MM/yyyy}", yfecha);
            ViewBag.dfentrega_orco = string.Format("{0:dd/MM/yyyy}", yfecha);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter cc = new ObjectParameter("cc", typeof(int));

            int code = 0;
            string data = "";
            int fila = 0;
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
                        var mofView = JsonConvert.DeserializeObject<ordencompraView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_Orden_CompraCrea(mofView.ncode_docu, mofView.sseri_orco, mofView.snume_orco, DateTime.Parse(mofView.sfeordencompra_orco),
                                DateTime.Parse(mofView.sfevenci_orco), DateTime.Parse(mofView.sfentrega_orco), mofView.ncode_provee ,mofView.smone_orco, mofView.ntc_orco, mofView.ncode_fopago,
                                mofView.sobse_orco,"",mofView.nbrutoex_orco, mofView.nbrutoaf_orco,
                                mofView.ndctoex_orco, mofView.ndsctoaf_orco, mofView.nsubex_orco, mofView.nsubaf_orco, mofView.nigvex_orco,
                                mofView.nigvaf_orco, mofView.ntotaex_orco, mofView.ntotaaf_orco, mofView.ntotal_orco, mofView.ntotalMN_orco,
                                mofView.ntotalUs_orco, true, mofView.nvalIGV_orco, User.Identity.Name, mofView.ncode_alma, int.Parse(User.Identity.GetLocal()),
                                mofView.ncode_mone,mofView.ncode_dose,sw);


                            code = int.Parse(sw.Value.ToString());
                            //cccode = int.Parse(cc.Value.ToString());

                            if (mofView.ordencompraViewDetas != null)
                            {
                                foreach (ordencompraViewDeta item in mofView.ordencompraViewDetas)
                                {
                                    fila++;
                                    db.Pr_Orden_CompraDetaCrea(code, item.ncode_arti, item.ncant_orcodeta, item.npu_orcodeta,
                                        item.ndscto_orcodeta, item.ndscto2_orcodeta, item.nexon_orcodeta, item.nafecto_orcodeta,
                                        item.besafecto_orcodeta, item.ncode_alma, item.ndsctomax_orcodeta,
                                        item.ndsctomin_orcodeta, item.ndsctoporc_orcodeta, item.npuorigen_orcodeta);
                                };

                            }

                            db.Pr_KardexElimina("ocompra", code);

                            db.Pr_KardexCrea("OCOMPRA", 1039, "A", code, User.Identity.Name);
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

        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0403", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDEN_COMPRAS ORDEN_COMPRAS = await db.ORDEN_COMPRAS.FindAsync(id);
            if (ORDEN_COMPRAS == null)
            {
                return HttpNotFound();
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";

            ViewBag.smone_orco = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ORDEN_COMPRAS.smone_orco);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).
                Where(c => c.ntipo_confi == 5 && c.svalor_confi == "O"), "ncode_confi", "sdesc_confi", ORDEN_COMPRAS.ncode_docu);
            ViewBag.sseri_orco = new SelectList(db.Pr_DocSerie(1, User.Identity.Name, 0, ORDEN_COMPRAS.ncode_docu), "ncode_dose", "serie",ORDEN_COMPRAS.ncode_dose);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", ORDEN_COMPRAS.ncode_fopago);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", ORDEN_COMPRAS.ncode_alma);
            ViewBag.ncode_vende = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_vende", "VendeZona", ORDEN_COMPRAS.ncode_vende);
            ViewBag.cod_prove = ORDEN_COMPRAS.ncode_provee;
            ViewBag.sdesc_prove = ORDEN_COMPRAS.PROVEEDOR.sdesc_prove;
            ViewBag.sruc_prove = ORDEN_COMPRAS.PROVEEDOR.sruc_prove;
            ViewBag.dfeorco_orco = string.Format("{0:dd/MM/yyyy}", ORDEN_COMPRAS.dfeorco_orco);
            ViewBag.dfevenci_orco = string.Format("{0:dd/MM/yyyy}", ORDEN_COMPRAS.dfevenci_orco);
            ViewBag.dfentrega_orco = string.Format("{0:dd/MM/yyyy}", ORDEN_COMPRAS.dfentrega_orco);
            ViewBag.tc = ORDEN_COMPRAS.ntc_orco;

            return View(ORDEN_COMPRAS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            int xsw;
            long code;
            string data = "";
            int fila = 0;
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
                        var mofView = JsonConvert.DeserializeObject<ordencompraView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_Orden_CompraEdita(mofView.ncode_orco, mofView.ncode_docu, DateTime.Parse(mofView.sfeordencompra_orco),
                                DateTime.Parse(mofView.sfevenci_orco), mofView.ncode_provee,mofView.smone_orco, mofView.ntc_orco, mofView.ncode_fopago,
                                mofView.sobse_orco, "", mofView.nbrutoex_orco, mofView.nbrutoaf_orco,
                                mofView.ndctoex_orco, mofView.ndsctoaf_orco, mofView.nsubex_orco, mofView.nsubaf_orco, mofView.nigvex_orco,
                                mofView.nigvaf_orco, mofView.ntotaex_orco, mofView.ntotaaf_orco, mofView.ntotal_orco, mofView.ntotalMN_orco,
                                mofView.ntotalUs_orco, true, mofView.nvalIGV_orco, User.Identity.Name, mofView.ncode_alma,
                                int.Parse(User.Identity.GetLocal()), mofView.ncode_mone,mofView.ncode_dose, sw);


                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_orco;

                            if (mofView.ordencompraViewDetas != null)
                            {
                                foreach (ordencompraViewDeta item in mofView.ordencompraViewDetas)
                                {
                                    fila++;
                                    db.Pr_Orden_CompraDetaCrea(code, item.ncode_arti, item.ncant_orcodeta, item.npu_orcodeta,
                                        item.ndscto_orcodeta, item.ndscto2_orcodeta, item.nexon_orcodeta, item.nafecto_orcodeta,
                                        item.besafecto_orcodeta, item.ncode_alma, item.ndsctomax_orcodeta,
                                        item.ndsctomin_orcodeta, item.ndsctoporc_orcodeta, item.npuorigen_orcodeta);
                                };

                            }

                            db.Pr_Orden_CompraDetaEdita(code);

                            db.Pr_KardexElimina("OCOMPRA", code);
                            db.Pr_KardexCrea("OCOMPRA", 1039, "A", code, User.Identity.Name);

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

        public async Task<ActionResult> DeleteOrden_compra(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0404", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ORDEN_COMPRAS ORDEN_COMPRAS = await db.ORDEN_COMPRAS.FindAsync(id);
            if (ORDEN_COMPRAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_OrdenCompraElimina(id, sw);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
