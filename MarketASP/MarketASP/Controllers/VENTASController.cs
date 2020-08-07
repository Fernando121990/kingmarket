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

namespace MarketASP.Controllers
{
    [Authorize]
    public class VENTASController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: VENTAS
        public async Task<ActionResult> Index()
        {
            var vENTAS = db.VENTAS.Include(v => v.CLI_DIRE).Include(v => v.CLIENTE).Include(v => v.CONFIGURACION).Include(v => v.CONFIGURACION1);
            return View(await vENTAS.ToListAsync());
        }

        // GET: VENTAS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }
            return View(vENTAS);
        }

        // GET: VENTAS/Create
        public ActionResult Create()
        {
            ViewBag.smone_movi = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "svalor_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "svalor_confi", "sdesc_confi");
            ViewBag.smone_venta = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_venta,ncode_docu,sseri_venta,snume_venta,dfeventa_venta,dfevenci_venta,ncode_cliente,ncode_clidire,smone_venta,ntc_venta,ncode_fopago,sobse_venta,ncode_compra,ncode_profo,nbrutoex_venta,nbrutoaf_venta,ndctoex_venta,ndsctoaf_venta,nsubex_venta,nsubaf_venta,nigvex_venta,nigvaf_venta,ntotaex_venta,ntotaaf_venta,ntotal_venta,ntotalMN_venta,ntotalUs_venta,besta_venta,nvalIGV_venta,suser_venta,dfech_venta,susmo_venta,dfemo_venta")] VENTAS vENTAS)
        {
            if (ModelState.IsValid)
            {
                db.VENTAS.Add(vENTAS);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire", vENTAS.ncode_clidire);
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", vENTAS.ncode_cliente);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", vENTAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", vENTAS.ncode_fopago);
            return View(vENTAS);
        }

        // GET: VENTAS/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire", vENTAS.ncode_clidire);
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", vENTAS.ncode_cliente);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", vENTAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", vENTAS.ncode_fopago);
            return View(vENTAS);
        }

        // POST: VENTAS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_venta,ncode_docu,sseri_venta,snume_venta,dfeventa_venta,dfevenci_venta,ncode_cliente,ncode_clidire,smone_venta,ntc_venta,ncode_fopago,sobse_venta,ncode_compra,ncode_profo,nbrutoex_venta,nbrutoaf_venta,ndctoex_venta,ndsctoaf_venta,nsubex_venta,nsubaf_venta,nigvex_venta,nigvaf_venta,ntotaex_venta,ntotaaf_venta,ntotal_venta,ntotalMN_venta,ntotalUs_venta,besta_venta,nvalIGV_venta,suser_venta,dfech_venta,susmo_venta,dfemo_venta")] VENTAS vENTAS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vENTAS).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire", vENTAS.ncode_clidire);
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", vENTAS.ncode_cliente);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", vENTAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", vENTAS.ncode_fopago);
            return View(vENTAS);
        }

        // GET: VENTAS/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }
            return View(vENTAS);
        }

        // POST: VENTAS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            db.VENTAS.Remove(vENTAS);
            await db.SaveChangesAsync();
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
