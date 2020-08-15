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
    public class CTASPA_DETALLEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CTASPA_DETALLE
        public async Task<ActionResult> Index()
        {
            var cTASPA_DETALLE = db.CTASPA_DETALLE.Include(c => c.CONFIGURACION).Include(c => c.CONFIGURACION1).Include(c => c.CTAS_PAGAR);
            return View(await cTASPA_DETALLE.ToListAsync());
        }

        // GET: CTASPA_DETALLE/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTASPA_DETALLE cTASPA_DETALLE = await db.CTASPA_DETALLE.FindAsync(id);
            if (cTASPA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(cTASPA_DETALLE);
        }

        // GET: CTASPA_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            ViewBag.ncode_ctapa = new SelectList(db.CTAS_PAGAR, "ncode_ctapa", "sdocu_ctapa");
            return View();
        }

        // POST: CTASPA_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_ctapade,dfepago_ctapade,smone_ctapade,ntc_ctapade,nmonto_ctapade,sobse_ctapade,nvuelto_ctapade,suser_ctapade,dfech_ctapade,susmo_ctapade,dfemo_ctapade,suser_tarjeta,snro_tarjeta,ncode_ctapa,ncode_banco,ncode_tpago")] CTASPA_DETALLE cTASPA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.CTASPA_DETALLE.Add(cTASPA_DETALLE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASPA_DETALLE.ncode_banco);
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASPA_DETALLE.ncode_tpago);
            ViewBag.ncode_ctapa = new SelectList(db.CTAS_PAGAR, "ncode_ctapa", "sdocu_ctapa", cTASPA_DETALLE.ncode_ctapa);
            return View(cTASPA_DETALLE);
        }

        // GET: CTASPA_DETALLE/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTASPA_DETALLE cTASPA_DETALLE = await db.CTASPA_DETALLE.FindAsync(id);
            if (cTASPA_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASPA_DETALLE.ncode_banco);
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASPA_DETALLE.ncode_tpago);
            ViewBag.ncode_ctapa = new SelectList(db.CTAS_PAGAR, "ncode_ctapa", "sdocu_ctapa", cTASPA_DETALLE.ncode_ctapa);
            return View(cTASPA_DETALLE);
        }

        // POST: CTASPA_DETALLE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_ctapade,dfepago_ctapade,smone_ctapade,ntc_ctapade,nmonto_ctapade,sobse_ctapade,nvuelto_ctapade,suser_ctapade,dfech_ctapade,susmo_ctapade,dfemo_ctapade,suser_tarjeta,snro_tarjeta,ncode_ctapa,ncode_banco,ncode_tpago")] CTASPA_DETALLE cTASPA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTASPA_DETALLE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASPA_DETALLE.ncode_banco);
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTASPA_DETALLE.ncode_tpago);
            ViewBag.ncode_ctapa = new SelectList(db.CTAS_PAGAR, "ncode_ctapa", "sdocu_ctapa", cTASPA_DETALLE.ncode_ctapa);
            return View(cTASPA_DETALLE);
        }

        // GET: CTASPA_DETALLE/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTASPA_DETALLE cTASPA_DETALLE = await db.CTASPA_DETALLE.FindAsync(id);
            if (cTASPA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(cTASPA_DETALLE);
        }

        // POST: CTASPA_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            CTASPA_DETALLE cTASPA_DETALLE = await db.CTASPA_DETALLE.FindAsync(id);
            db.CTASPA_DETALLE.Remove(cTASPA_DETALLE);
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
