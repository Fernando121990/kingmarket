using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarketASP.Models;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;
using MarketASP.Areas.Inventario.Clases;
using MarketASP.Clases;

namespace MarketASP.Areas.Inventario.Controllers
{
    [Authorize]
    public class MOVIMIENTOesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0201", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var mOVIMIENTO = db.MOVIMIENTO.Include(m => m.ALMACEN).Include(m => m.ALMACEN1).Include(m => m.TIPO_MOVIMIENTO);
            return View(await mOVIMIENTO.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            if (mOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(mOVIMIENTO);
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0202", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");


            var rtimovi = from s in db.TIPO_MOVIMIENTO
                         where (s.besta_tipomovi == true)
                         select new { s.ncode_timovi, sdesc_timovi = s.stipo_timovi + " " + s.sdesc_timovi };

            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma==true), "ncode_alma", "sdesc_alma");
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ncode_timovi = new SelectList(rtimovi, "ncode_timovi", "sdesc_timovi");
            ViewBag.smone_movi = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ViewBag.moneda);

            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc ==  yfecha);
            if (result == null)
            {
                return RedirectToAction("Create","Tipo_Cambio",new {area=""});
            }
            ViewBag.tc = result.nventa_tc;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            int code;
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
                        var mofView = JsonConvert.DeserializeObject<moviView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_movimientoCrear(DateTime.Parse(mofView.dfemov_movi), mofView.smone_movi, mofView.ntc_movi, mofView.sobse_movi,
                                "", "", User.Identity.Name, mofView.ncode_timovi, mofView.ncode_alma, mofView.ndestino_alma,mofView.stipo_movi , sw);

                            code = int.Parse(sw.Value.ToString());

                            if (mofView.moviViewDetas != null)
                            {
                                foreach (moviViewDeta item in mofView.moviViewDetas)
                                {
                                    fila++;
                                    db.Pr_movimientoDetaCrea(item.ncode_arti, item.ncant_movidet, item.npu_movidet, User.Identity.Name, code,item.ncode_umed);
                                };

                            }

                            if (mofView.moviViewLotes != null)
                            {
                                foreach (moviViewLote item in mofView.moviViewLotes)
                                {
                                    fila++;
                                    db.Pr_MoviLoteCrea(code, item.ncode_arti, item.ncant_movilote, item.ncode_alma,
                                        item.sdesc_lote, DateTime.Parse(item.sfvenci_lote), item.ncode_lote);
                                };

                            }

                            db.Pr_KardexCrea("Movimiento", 4, mofView.stipo_movi, code, User.Identity.Name);

                            db.Pr_LoteCrear("", null, 0, 0, 0, User.Identity.Name, "MOVIMIENTO", 4, mofView.stipo_movi, code, 0, "", "", code, mensaje, sw);


                            
                        }
                    }
                }
                return Json(new { Success = 1, Mensaje = "Movimiento Registrado" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0, Mensaje = mensajex });
            }
        }


        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0203", xcode);
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
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            if (mOVIMIENTO == null)
            {
                return HttpNotFound();
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");


            var rtimovi = from s in db.TIPO_MOVIMIENTO
                          where (s.besta_tipomovi == true)
                          select new { s.ncode_timovi, sdesc_timovi = s.stipo_timovi + " " + s.sdesc_timovi };

            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ndestino_alma);
            ViewBag.ncode_timovi = new SelectList(rtimovi, "ncode_timovi", "sdesc_timovi", mOVIMIENTO.ncode_timovi);
            ViewBag.smone_movi = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi",mOVIMIENTO.smone_movi);
            ViewBag.tc = mOVIMIENTO.ntc_movi;
            return View(mOVIMIENTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            string data = "";
            int fila = 0;
            int code = 0;
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
                        var mofView = JsonConvert.DeserializeObject<moviView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_movimientoEditar(mofView.ncode_movi, DateTime.Parse(mofView.dfemov_movi), mofView.smone_movi, mofView.ntc_movi, mofView.sobse_movi,
                                "", "", User.Identity.Name, mofView.ncode_timovi, mofView.ncode_alma, mofView.ndestino_alma,mofView.stipo_movi, false,sw);
                            
                            code = (int)mofView.ncode_movi;

                            //db.Pr_movimientoDetaElimina(mofView.ncode_movi);

                            if (mofView.moviViewDetas != null)
                            {
                                foreach (moviViewDeta item in mofView.moviViewDetas)
                                {
                                    fila++;
                                    db.Pr_movimientoDetaCrea(item.ncode_arti, item.ncant_movidet, item.npu_movidet, User.Identity.Name, mofView.ncode_movi, item.ncode_umed);
                                };

                            }

                            if (mofView.moviViewLotes != null)
                            {
                                foreach (moviViewLote item in mofView.moviViewLotes)
                                {
                                    fila++;
                                    db.Pr_MoviLoteCrea(code, item.ncode_arti, item.ncant_movilote, item.ncode_alma,
                                        item.sdesc_lote, DateTime.Parse(item.sfvenci_lote), item.ncode_lote);
                                };

                            }

                            db.Pr_MoviDetaEdita(code);

                            db.Pr_KardexElimina("movimiento", code);

                            db.Pr_KardexCrea("movimiento", 4, mofView.stipo_movi, code, User.Identity.Name);

                            db.Pr_LoteElimina("movimiento", code);

                            db.Pr_LoteCrear("", null, 0, 0, 0, User.Identity.Name, "movimiento", 4, mofView.stipo_movi, code, 0, "", "", code, mensaje, sw);
                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Movimiento Registrado" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0, Mensaje = mensajex });
            }
        }
        public ActionResult DeleteMovi(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0204", xcode);
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

            db.Pr_movimientoElimina(id);
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
