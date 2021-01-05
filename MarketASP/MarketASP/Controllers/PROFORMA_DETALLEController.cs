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
    public class PROFORMA_DETALLEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: PROFORMA_DETALLE
        public async Task<ActionResult> Index()
        {
            var pROFORMA_DETALLE = db.PROFORMA_DETALLE.Include(p => p.ARTICULO).Include(p => p.PROFORMAS);
            return View(await pROFORMA_DETALLE.ToListAsync());
        }

        // GET: PROFORMA_DETALLE/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROFORMA_DETALLE pROFORMA_DETALLE = await db.PROFORMA_DETALLE.FindAsync(id);
            if (pROFORMA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(pROFORMA_DETALLE);
        }

        // GET: PROFORMA_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            ViewBag.ncode_prof = new SelectList(db.PROFORMAS, "ncode_prof", "sseri_prof");
            return View();
        }

        // POST: PROFORMA_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_profdeta,ncode_arti,ncant_profdeta,npu_profdeta,ndscto_profdeta,ndscto2_profdeta,nexon_profdeta,nafecto_profdeta,besafecto_profdeta,ncode_alma,ndsctomax_profdeta,ndsctomin_profdeta,ndsctoporc_profdeta,nback_profdeta,ncode_prof")] PROFORMA_DETALLE pROFORMA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.PROFORMA_DETALLE.Add(pROFORMA_DETALLE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", pROFORMA_DETALLE.ncode_arti);
            ViewBag.ncode_prof = new SelectList(db.PROFORMAS, "ncode_prof", "sseri_prof", pROFORMA_DETALLE.ncode_prof);
            return View(pROFORMA_DETALLE);
        }

        // GET: PROFORMA_DETALLE/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROFORMA_DETALLE pROFORMA_DETALLE = await db.PROFORMA_DETALLE.FindAsync(id);
            if (pROFORMA_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", pROFORMA_DETALLE.ncode_arti);
            ViewBag.ncode_prof = new SelectList(db.PROFORMAS, "ncode_prof", "sseri_prof", pROFORMA_DETALLE.ncode_prof);
            return View(pROFORMA_DETALLE);
        }

        // POST: PROFORMA_DETALLE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_profdeta,ncode_arti,ncant_profdeta,npu_profdeta,ndscto_profdeta,ndscto2_profdeta,nexon_profdeta,nafecto_profdeta,besafecto_profdeta,ncode_alma,ndsctomax_profdeta,ndsctomin_profdeta,ndsctoporc_profdeta,nback_profdeta,ncode_prof")] PROFORMA_DETALLE pROFORMA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pROFORMA_DETALLE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", pROFORMA_DETALLE.ncode_arti);
            ViewBag.ncode_prof = new SelectList(db.PROFORMAS, "ncode_prof", "sseri_prof", pROFORMA_DETALLE.ncode_prof);
            return View(pROFORMA_DETALLE);
        }

        // GET: PROFORMA_DETALLE/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROFORMA_DETALLE pROFORMA_DETALLE = await db.PROFORMA_DETALLE.FindAsync(id);
            if (pROFORMA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(pROFORMA_DETALLE);
        }

        // POST: PROFORMA_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            PROFORMA_DETALLE pROFORMA_DETALLE = await db.PROFORMA_DETALLE.FindAsync(id);
            db.PROFORMA_DETALLE.Remove(pROFORMA_DETALLE);
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
