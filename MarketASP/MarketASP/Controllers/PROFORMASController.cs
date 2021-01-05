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
using MarketASP.Clases;
using Newtonsoft.Json;
using MarketASP.Extensiones;

namespace MarketASP.Controllers
{
    public class PROFORMASController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: PROFORMAS
        public async Task<ActionResult> Index()
        {
            var pROFORMAS = db.PROFORMAS.Include(p => p.ALMACEN).Include(p => p.CLI_DIRE).Include(p => p.CLIENTE).Include(p => p.CONFIGURACION).Include(p => p.CONFIGURACION1).Include(p => p.LOCAL);
            return View(await pROFORMAS.ToListAsync());
        }

        // GET: PROFORMAS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROFORMAS pROFORMAS = await db.PROFORMAS.FindAsync(id);
            if (pROFORMAS == null)
            {
                return HttpNotFound();
            }
            return View(pROFORMAS);
        }

        // GET: PROFORMAS/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1302", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.smone_movi = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                ViewBag.mensaje = "No se ha registrado el tipo de cambio, comuniquese con el administrador del sistema";
                return View("_Mensaje");
                //                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5 && c.ncode_confi == 1032), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.smone_prof = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
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
                        var mofView = JsonConvert.DeserializeObject<proformaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_proformaCrea(mofView.ncode_docu, mofView.sseri_prof, mofView.snume_prof, DateTime.Parse(mofView.sfeprofo_prof),
                                DateTime.Parse(mofView.sfevenci_prof), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_prof, mofView.ntc_prof, mofView.ncode_fopago,
                                mofView.sobse_prof, mofView.ncode_compra, mofView.nbrutoex_prof, mofView.nbrutoaf_prof,
                                mofView.ndctoex_prof, mofView.ndsctoaf_prof, mofView.nsubex_prof, mofView.nsubaf_prof, mofView.nigvex_prof,
                                mofView.nigvaf_prof, mofView.ntotaex_prof, mofView.ntotaaf_prof, mofView.ntotal_prof, mofView.ntotalMN_prof,
                                mofView.ntotalUs_prof, true, mofView.nvalIGV_prof, User.Identity.Name, mofView.ncode_alma, int.Parse(User.Identity.GetLocal()), mofView.ncode_mone ,
                                sw);


                            code = int.Parse(sw.Value.ToString());
                            //cccode = int.Parse(cc.Value.ToString());

                            if (mofView.proformaViewDetas != null)
                            {
                                foreach (proformaViewDeta item in mofView.proformaViewDetas)
                                {
                                    fila++;
                                    db.Pr_proformaDetaCrea(code, item.ncode_arti, item.ncant_profdeta, item.npu_profdeta,
                                        item.ndscto_profdeta, item.ndscto2_profdeta, item.nexon_profdeta, item.nafecto_profdeta, item.besafecto_profdeta,
                                        item.ncode_alma, item.ndsctomax_profdeta, item.ndsctomin_profdeta, item.ndsctoporc_profdeta);
                                };

                            }

                            //db.Pr_KardexCrea("Venta", 5, "S", code, User.Identity.Name);
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

            db.Pr_PermisoAcceso(User.Identity.Name, "1303", xcode);
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
            PROFORMAS pROFORMAS = await db.PROFORMAS.FindAsync(id);
            if (pROFORMAS == null)
            {
                return HttpNotFound();
            }
            ViewBag.smone_prof = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", pROFORMAS.smone_prof);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5 && c.ncode_confi == 1032), "ncode_confi", "sdesc_confi", pROFORMAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", pROFORMAS.ncode_fopago);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", pROFORMAS.ncode_alma);
            ViewBag.sdesc_cliente = pROFORMAS.CLIENTE.srazon_cliente;
            ViewBag.sruc_cliente = pROFORMAS.CLIENTE.sruc_cliente;
            ViewBag.sdni_cliente = pROFORMAS.CLIENTE.sdnice_cliente;
            ViewBag.NRO_DCLIENTE = new SelectList(db.CLI_DIRE.Where(c => c.ncode_cliente == pROFORMAS.ncode_cliente), "ncode_clidire", "sdesc_clidire", pROFORMAS.ncode_clidire);
            return View(pROFORMAS);
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
                        var mofView = JsonConvert.DeserializeObject<proformaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_proformaEdita(mofView.ncode_prof, mofView.ncode_docu, DateTime.Parse(mofView.sfeprofo_prof),
                                DateTime.Parse(mofView.sfevenci_prof), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_prof, mofView.ntc_prof, mofView.ncode_fopago,
                                mofView.sobse_prof, mofView.ncode_compra, mofView.nbrutoex_prof, mofView.nbrutoaf_prof,
                                mofView.ndctoex_prof, mofView.ndsctoaf_prof, mofView.nsubex_prof, mofView.nsubaf_prof, mofView.nigvex_prof,
                                mofView.nigvaf_prof, mofView.ntotaex_prof, mofView.ntotaaf_prof, mofView.ntotal_prof, mofView.ntotalMN_prof,
                                mofView.ntotalUs_prof,true, mofView.nvalIGV_prof, User.Identity.Name, mofView.ncode_alma, int.Parse(User.Identity.GetLocal()), mofView.ncode_mone, sw);


                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_prof;

                            if (mofView.proformaViewDetas != null)
                            {
                                foreach (proformaViewDeta item in mofView.proformaViewDetas)
                                {
                                    fila++;
                                    db.Pr_proformaDetaCrea(code, item.ncode_arti, item.ncant_profdeta, item.npu_profdeta,
                                        item.ndscto_profdeta, item.ndscto2_profdeta, item.nexon_profdeta, item.nafecto_profdeta, item.besafecto_profdeta,
                                        item.ncode_alma, item.ndsctomax_profdeta, item.ndsctomin_profdeta, item.ndsctoporc_profdeta);
                                };

                            }

                            db.Pr_proformaDetaEdita(code);

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

        //public async Task<ActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PROFORMAS pROFORMAS = await db.PROFORMAS.FindAsync(id);
        //    if (pROFORMAS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(pROFORMAS);
        //}

        //// POST: PROFORMAS/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    PROFORMAS pROFORMAS = await db.PROFORMAS.FindAsync(id);
        //    db.PROFORMAS.Remove(pROFORMAS);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        public async Task<ActionResult> DeleteProforma(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1304", xcode);
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

            PROFORMAS pROFORMAS = await db.PROFORMAS.FindAsync(id);
            if (pROFORMAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_proformaElimina(id, sw);
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
