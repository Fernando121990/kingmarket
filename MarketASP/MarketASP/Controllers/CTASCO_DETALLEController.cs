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
    public class CTASCO_DETALLEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CTASCO_DETALLE
        public async Task<ActionResult> Index()
        {
            var cTASCO_DETALLE = db.CTASCO_DETALLE.Include(c => c.CONFIGURACION).Include(c => c.CONFIGURACION1).Include(c => c.CTA_COBRAR);
            return View(await cTASCO_DETALLE.ToListAsync());
        }

        // GET: CTASCO_DETALLE/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTASCO_DETALLE cTASCO_DETALLE = await db.CTASCO_DETALLE.FindAsync(id);
            if (cTASCO_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(cTASCO_DETALLE);
        }

        // GET: CTASCO_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            ViewBag.ncode_ctaco = new SelectList(db.CTA_COBRAR, "ncode_ctaco", "sdocu_ctaco");
            return View();
        }

        // POST: CTASCO_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_ctacode,dfepago_ctacode,sdoc_ctacode,smone_ctacode,ntc_ctacode,nmonto_ctacode,nvuelto_ctacode,suser_tarjeta,snro_tarjeta,sobse_ctacode,suser_ctacode,dfech_ctacode,susmo_ctacode,dfemo_ctacode,ncode_ctaco,ncode_banco,ncode_tpago")] CTASCO_DETALLE cTASCO_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.CTASCO_DETALLE.Add(cTASCO_DETALLE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASCO_DETALLE.ncode_banco);
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASCO_DETALLE.ncode_tpago);
            ViewBag.ncode_ctaco = new SelectList(db.CTA_COBRAR, "ncode_ctaco", "sdocu_ctaco", cTASCO_DETALLE.ncode_ctaco);
            return View(cTASCO_DETALLE);
        }

        // GET: CTASCO_DETALLE/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTASCO_DETALLE cTASCO_DETALLE = await db.CTASCO_DETALLE.FindAsync(id);
            if (cTASCO_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASCO_DETALLE.ncode_banco);
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASCO_DETALLE.ncode_tpago);
            ViewBag.ncode_ctaco = new SelectList(db.CTA_COBRAR, "ncode_ctaco", "sdocu_ctaco", cTASCO_DETALLE.ncode_ctaco);
            return View(cTASCO_DETALLE);
        }

        // POST: CTASCO_DETALLE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_ctacode,dfepago_ctacode,sdoc_ctacode,smone_ctacode,ntc_ctacode,nmonto_ctacode,nvuelto_ctacode,suser_tarjeta,snro_tarjeta,sobse_ctacode,suser_ctacode,dfech_ctacode,susmo_ctacode,dfemo_ctacode,ncode_ctaco,ncode_banco,ncode_tpago")] CTASCO_DETALLE cTASCO_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTASCO_DETALLE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASCO_DETALLE.ncode_banco);
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASCO_DETALLE.ncode_tpago);
            ViewBag.ncode_ctaco = new SelectList(db.CTA_COBRAR, "ncode_ctaco", "sdocu_ctaco", cTASCO_DETALLE.ncode_ctaco);
            return View(cTASCO_DETALLE);
        }

        // GET: CTASCO_DETALLE/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTASCO_DETALLE cTASCO_DETALLE = await db.CTASCO_DETALLE.FindAsync(id);
            if (cTASCO_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(cTASCO_DETALLE);
        }

        // POST: CTASCO_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            CTASCO_DETALLE cTASCO_DETALLE = await db.CTASCO_DETALLE.FindAsync(id);
            db.CTASCO_DETALLE.Remove(cTASCO_DETALLE);
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
