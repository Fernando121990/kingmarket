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
using MarketASP.Clases;

namespace MarketASP.Controllers
{
    public class COMPRASController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: COMPRAS
        public async Task<ActionResult> Index()
        {
            var cOMPRAS = db.COMPRAS.Include(c => c.ALMACEN).Include(c => c.PROVEEDOR).Include(c => c.CONFIGURACION).Include(c => c.CONFIGURACION1);
            return View(await cOMPRAS.ToListAsync());
        }

        // GET: COMPRAS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }
            return View(cOMPRAS);
        }

        // GET: COMPRAS/Create
        public ActionResult Create()
        {
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.smone_compra = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

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
                        var mofView = JsonConvert.DeserializeObject<compraView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_compraCrea( mofView.sseri_compra, mofView.snume_compra, DateTime.Parse(mofView.sfecompra_compra),
                                DateTime.Parse(mofView.sfevenci_compra), mofView.smone_compra, mofView.ntc_compra, mofView.sobs_compra,
                                mofView.sguia_compra, mofView.sproforma_compra, mofView.nbrutoex_compra, mofView.nbrutoaf_compra,
                                mofView.ndsctoex_compra, mofView.ndsctoaf_compra, mofView.nsubex_compra, mofView.nsubaf_compra, mofView.nigvex_compra,
                                mofView.nigvaf_compra, mofView.ntotaex_compra, mofView.ntotaaf_compra, mofView.ntotal_compra, mofView.ntotalMN_compra,
                                mofView.ntotalUS_compra,mofView.nvalIGV_compra, User.Identity.Name, mofView.ncode_alma,mofView.ncode_provee,
                                mofView.ncode_docu,mofView.ncode_fopago, sw);


                            code = int.Parse(sw.Value.ToString());

                            if (mofView.compraViewDetas != null)
                            {
                                foreach (compraViewDeta item in mofView.compraViewDetas)
                                {
                                    fila++;
                                    db.Pr_compraDetaCrea(item.ncant_comdeta, item.npu_comdeta,
                                        item.ndscto_comdeta, item.ndscto2_comdeta, item.nexon_comdeta, item.nafecto_comdeta, item.besafecto_comdeta,
                                        code, item.ncode_alma, item.ncode_arti);
                                };

                            }

                            db.Pr_KardexCrea("Compra", 6, "I", code, User.Identity.Name);
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi", cOMPRAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", cOMPRAS.ncode_fopago);
            ViewBag.smone_compra = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", cOMPRAS.smone_compra);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", cOMPRAS.ncode_alma);
            ViewBag.sdesc_prove = cOMPRAS.PROVEEDOR.sdesc_prove;
            return View(cOMPRAS);
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
                        var mofView = JsonConvert.DeserializeObject<compraView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_compraEdita(mofView.ncode_compra ,mofView.sseri_compra, mofView.snume_compra, DateTime.Parse(mofView.sfecompra_compra),
                                DateTime.Parse(mofView.sfevenci_compra), mofView.smone_compra, mofView.ntc_compra, mofView.sobs_compra,
                                mofView.sguia_compra, mofView.sproforma_compra, mofView.nbrutoex_compra, mofView.nbrutoaf_compra,
                                mofView.ndsctoex_compra, mofView.ndsctoaf_compra, mofView.nsubex_compra, mofView.nsubaf_compra, mofView.nigvex_compra,
                                mofView.nigvaf_compra, mofView.ntotaex_compra, mofView.ntotaaf_compra, mofView.ntotal_compra, mofView.ntotalMN_compra,
                                mofView.ntotalUS_compra, mofView.nvalIGV_compra, User.Identity.Name, mofView.ncode_alma, mofView.ncode_provee,
                                mofView.ncode_docu, mofView.ncode_fopago, sw);

                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_compra;

                            if (mofView.compraViewDetas != null)
                            {
                                foreach (compraViewDeta item in mofView.compraViewDetas)
                                {
                                    fila++;
                                    db.Pr_compraDetaCrea(item.ncant_comdeta, item.npu_comdeta,
                                        item.ndscto_comdeta, item.ndscto2_comdeta, item.nexon_comdeta, item.nafecto_comdeta, item.besafecto_comdeta,
                                        code, item.ncode_alma, item.ncode_arti);
                                };

                            }

                            db.Pr_KardexCrea("Compra", 6, "I", code, User.Identity.Name);

                            db.Pr_ventaDetaEdita(code);
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

        public async Task<ActionResult> anulaCompra(int? id)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_compraAnula(id, sw);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteCompra(int? id)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_compraElimina(id, sw);
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
