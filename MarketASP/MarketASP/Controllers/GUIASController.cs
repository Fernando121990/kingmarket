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
            ViewBag.poretencion = Helpers.Funciones.ObtenerValorParam("GENERAL", "% RETENCION");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";

            var rtiguia = from s in db.TIPO_GUIA
                          where (s.besta_tiguia == true)
                          select new { s.ncode_tiguia, sdesc_tiguia = s.stipo_tiguia + " " + s.sdesc_tiguia };
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).
                Where(c => c.ntipo_confi == 5).Where(c => c.svalor_confi == "G"), "ncode_confi", "sdesc_confi",1070);
            ViewBag.sserie_guia = new SelectList(db.Pr_DocSerie(1, User.Identity.Name, 0, 1070), "ncode_dose", "serie");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ncode_tiguia = new SelectList(rtiguia, "ncode_tiguia", "sdesc_tiguia");
            ViewBag.ncode_tran = new SelectList(db.TRANSPORTISTA.Where(t=>t.nesta_tran == true), "ncode_tran", "snomb_tran");
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
                                mofView.stipo_guia,mofView.ncode_cliente,mofView.ncode_clidire,mofView.ncode_docu,mofView.ncode_mone,
                                mofView.ncode_tran, mofView.ncode_orpe, mofView.sserienume_orpe,mofView.ncode_dose,
                                mofView.nbrutoex_guia,mofView.nbrutoaf_guia,mofView.ndsctoex_guia,mofView.ndsctoaf_guia,
                                mofView.nsubex_guia,mofView.nsubaf_guia,mofView.nigvex_guia,mofView.nigvaf_guia,
                                mofView.ntotaex_guia,mofView.ntotaaf_guia,mofView.ntotal_guia,mofView.ntotalMN_guia,
                                mofView.ntotalUS_guia,mofView.nvalIGV_guia,mofView.bclienteagretencion,mofView.ncuotas_guia,
                                mofView.ncuotavalor_guia,mofView.ncuotadias_guia,mofView.sglosadespacho_guia,mofView.bflete_guia, sw);

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

                            if (mofView.guiaViewCuotas != null)
                            {
                                foreach (guiaViewCuota item in mofView.guiaViewCuotas)
                                {
                                    fila++;
                                    db.Pr_GuiaCuotaCrud("C", 0, code, DateTime.Parse(item.sfecharegistro), item.nvalor_guiacu, User.Identity.Name, sw);
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
            ViewBag.poretencion = Helpers.Funciones.ObtenerValorParam("GENERAL", "% RETENCION");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";

            var rtiguia = from s in db.TIPO_GUIA
                          where (s.besta_tiguia == true)
                          select new { s.ncode_tiguia, sdesc_tiguia = s.stipo_tiguia + " " + s.sdesc_tiguia };

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).
                Where(c => c.ntipo_confi == 5).Where(c => c.svalor_confi == "G"), "ncode_confi", "sdesc_confi",gUIA.ncode_docu);
            ViewBag.sserie_guia = new SelectList(db.Pr_DocSerie(1, User.Identity.Name, 0, gUIA.ncode_docu), "ncode_dose", "serie",gUIA.ncode_dose);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma",gUIA.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma", gUIA.ndestino_alma);
            ViewBag.ncode_tiguia = new SelectList(rtiguia, "ncode_tiguia", "sdesc_tiguia", gUIA.ncode_tiguia);
            ViewBag.smone_guia = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", gUIA.smone_guia);
            ViewBag.ncode_tran = new SelectList(db.TRANSPORTISTA.Where(t => t.nesta_tran == true), "ncode_tran", "snomb_tran",gUIA.ncode_tran);
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
                                "", "", User.Identity.Name, mofView.ncode_tiguia, mofView.ncode_alma, mofView.ndestino_alma, mofView.stipo_guia, false,
                                mofView.ncode_tran,mofView.ncode_orpe,mofView.sserienume_orpe,mofView.ncode_dose,
                                                                mofView.nbrutoex_guia, mofView.nbrutoaf_guia, mofView.ndsctoex_guia, mofView.ndsctoaf_guia,
                                mofView.nsubex_guia, mofView.nsubaf_guia, mofView.nigvex_guia, mofView.nigvaf_guia,
                                mofView.ntotaex_guia, mofView.ntotaaf_guia, mofView.ntotal_guia, mofView.ntotalMN_guia,
                                mofView.ntotalUS_guia, mofView.nvalIGV_guia, mofView.bclienteagretencion, mofView.ncuotas_guia,
                                mofView.ncuotavalor_guia, mofView.ncuotadias_guia, mofView.sglosadespacho_guia, mofView.bflete_guia, sw);


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

                            if (mofView.guiaViewCuotas != null)
                            {
                                db.Pr_GuiaCuotaCrud("D", 0, code, DateTime.Parse(mofView.sfemov_guia), 0, "", sw);

                                foreach (guiaViewCuota item in mofView.guiaViewCuotas)
                                {
                                    fila++;
                                    db.Pr_GuiaCuotaCrud("C", 0, code, DateTime.Parse(item.sfecharegistro), item.nvalor_guiacu, User.Identity.Name, sw);

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