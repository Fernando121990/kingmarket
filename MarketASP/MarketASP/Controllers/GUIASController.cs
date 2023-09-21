using MarketASP.Areas.Inventario.Clases;
using MarketASP.Clases;
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
    public class GUIASController : Controller
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

            var gUIA = db.GUIA.ToList();
            return View(gUIA);
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GUIA gUIA = await db.GUIA.FindAsync(id);
            if (gUIA == null)
            {
                return HttpNotFound();
            }
            return View(gUIA);
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


            var rtiguia = from s in db.TIPO_GUIA
                          where (s.besta_tiguia == true)
                          select new { s.ncode_tiguia, sdesc_tiguia = s.stipo_tiguia + " " + s.sdesc_tiguia };
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5).Where(c => c.svalor_confi == "G"), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ncode_tiguia = new SelectList(rtiguia, "ncode_tiguia", "sdesc_tiguia");
            ViewBag.smone_guia = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ViewBag.moneda);

            var yfecha = DateTime.Now.Date;
            ViewBag.dfemov_guia = string.Format("{0:dd/MM/yyyy}", yfecha);
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
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
                        var mofView = JsonConvert.DeserializeObject<guiaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_GUIACrear(DateTime.Parse(mofView.sfemov_guia), mofView.smone_guia, mofView.ntc_guia, mofView.sobse_guia,
                                mofView.sserie_guia,mofView.snume_guia, User.Identity.Name, mofView.ncode_tiguia, mofView.ncode_alma, mofView.ndestino_alma, 
                                mofView.stipo_guia,mofView.ncode_cliente,mofView.ncode_clidire,mofView.ncode_docu,mofView.ncode_mone, sw);

                            code = int.Parse(sw.Value.ToString());

                            if (mofView.guiaViewDetas != null)
                            {
                                foreach (guiaViewDeta item in mofView.guiaViewDetas)
                                {
                                    fila++;
                                    db.Pr_GuiaDetaCrea(item.ncode_arti, item.ncant_guiadet, item.npu_guiadet, User.Identity.Name, code,(int) item.ncode_umed);
                                };

                            }

                            if (mofView.guiaViewLotes != null)
                            {
                                foreach (guiaViewLote item in mofView.guiaViewLotes)
                                {
                                    fila++;
                                    db.Pr_GuiaLoteCrea(code, item.ncode_arti, item.ncant_guialote, item.ncode_alma,
                                        item.sdesc_lote, DateTime.Parse(item.sfvenci_lote), item.ncode_lote);
                                };

                            }

                            db.Pr_KardexCrea("GUIA", 4, mofView.stipo_guia, code, User.Identity.Name);
                            
                            db.Pr_LoteCrear("", null, 0, 0, 0, User.Identity.Name, "GUIA", 4, mofView.stipo_guia , code, 0, "", "", code, mensaje, sw);

                        }
                    }
                }
                
                return Json(new { Success = 1, Mensaje = "Guia Registrada" });

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
            GUIA gUIA = await db.GUIA.FindAsync(id);
            if (gUIA == null)
            {
                return HttpNotFound();
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");

            var rtiguia = from s in db.TIPO_GUIA
                          where (s.besta_tiguia == true)
                          select new { s.ncode_tiguia, sdesc_tiguia = s.stipo_tiguia + " " + s.sdesc_tiguia };

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5).Where(c => c.svalor_confi == "G"), "ncode_confi", "sdesc_confi",gUIA.ncode_docu);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma",gUIA.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma", gUIA.ndestino_alma);
            ViewBag.ncode_tiguia = new SelectList(rtiguia, "ncode_tiguia", "sdesc_tiguia", gUIA.ncode_tiguia);
            ViewBag.smone_guia = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", gUIA.smone_guia);
            ViewBag.tc = gUIA.ntc_guia;
            ViewBag.dfemov_guia = string.Format("{0:dd/MM/yyyy}", gUIA.dfemov_guia);
            CLIENTE cLIENTE = db.CLIENTE.FirstOrDefault(x => x.ncode_cliente == gUIA.ncode_cliente);
            ViewBag.cod_cliente = gUIA.ncode_cliente;
            ViewBag.sdesc_cliente = cLIENTE.srazon_cliente;
            ViewBag.sruc_cliente = cLIENTE.sruc_cliente;
            ViewBag.sdni_cliente = cLIENTE.sdnice_cliente;

            return View(gUIA);
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
                        var mofView = JsonConvert.DeserializeObject<guiaView>(data, jsonSettings);

                        if (mofView != null)
                        {


                            db.Pr_GuiaEditar(mofView.ncode_guia,DateTime.Parse(mofView.sfemov_guia), mofView.smone_guia, mofView.ntc_guia, mofView.sobse_guia,
                                "", "", User.Identity.Name, mofView.ncode_tiguia, mofView.ncode_alma, mofView.ndestino_alma, mofView.stipo_guia, false, sw);

                            code = (int) mofView.ncode_guia;

                            //db.Pr_GuiaDetaElimina(mofView.ncode_guia);

                            if (mofView.guiaViewDetas != null)
                            {
                                foreach (guiaViewDeta item in mofView.guiaViewDetas)
                                {
                                    fila++;
                                    db.Pr_GuiaDetaCrea(item.ncode_arti, item.ncant_guiadet, item.npu_guiadet, User.Identity.Name,code,(int) item.ncode_umed);
                                };

                            }

                            if (mofView.guiaViewLotes != null)
                            {
                                foreach (guiaViewLote item in mofView.guiaViewLotes)
                                {
                                    fila++;
                                    db.Pr_GuiaLoteCrea(code, item.ncode_arti, item.ncant_guialote, item.ncode_alma,
                                        item.sdesc_lote, DateTime.Parse(item.sfvenci_lote), item.ncode_lote);
                                };

                            }

                            db.Pr_GuiaDetaEdita(code);

                            db.Pr_KardexElimina("guia", code);

                            db.Pr_KardexCrea("guia", 4, mofView.stipo_guia, code, User.Identity.Name);

                            db.Pr_LoteElimina("guia", code);

                            db.Pr_LoteCrear("", null, 0, 0, 0, User.Identity.Name, "guia", 4, mofView.stipo_guia, code, 0, "", "", code, mensaje, sw);

                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Guia Registrada" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                //ViewBag.mensaje = mensaje;
                return Json(new { Success = 0, Mensaje = mensajex });
            }
        }
        public ActionResult Deleteguia(int? id)
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

            db.Pr_GuiaElimina(id);
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