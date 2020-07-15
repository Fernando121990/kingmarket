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
    public class MOVI_DETALLEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: MOVI_DETALLE
        public async Task<ActionResult> Index()
        {
            var mOVI_DETALLE = db.MOVI_DETALLE.Include(m => m.ARTICULO).Include(m => m.MOVIMIENTO);
            return View(await mOVI_DETALLE.ToListAsync());
        }

        // GET: MOVI_DETALLE/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVI_DETALLE mOVI_DETALLE = await db.MOVI_DETALLE.FindAsync(id);
            if (mOVI_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(mOVI_DETALLE);
        }

        // GET: MOVI_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            ViewBag.ncode_movi = new SelectList(db.MOVIMIENTO, "ncode_movi", "smone_movi");
            return View();
        }

        // POST: MOVI_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_movidet,ncode_arti,ncant_movidet,npu_movidet,suser_movidet,dfech_movidet,susmo_movidet,dfemo_movidet,ncode_movi")] MOVI_DETALLE mOVI_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.MOVI_DETALLE.Add(mOVI_DETALLE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", mOVI_DETALLE.ncode_arti);
            ViewBag.ncode_movi = new SelectList(db.MOVIMIENTO, "ncode_movi", "smone_movi", mOVI_DETALLE.ncode_movi);
            return View(mOVI_DETALLE);
        }

        // GET: MOVI_DETALLE/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVI_DETALLE mOVI_DETALLE = await db.MOVI_DETALLE.FindAsync(id);
            if (mOVI_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", mOVI_DETALLE.ncode_arti);
            ViewBag.ncode_movi = new SelectList(db.MOVIMIENTO, "ncode_movi", "smone_movi", mOVI_DETALLE.ncode_movi);
            return View(mOVI_DETALLE);
        }

        // POST: MOVI_DETALLE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_movidet,ncode_arti,ncant_movidet,npu_movidet,suser_movidet,dfech_movidet,susmo_movidet,dfemo_movidet,ncode_movi")] MOVI_DETALLE mOVI_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mOVI_DETALLE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", mOVI_DETALLE.ncode_arti);
            ViewBag.ncode_movi = new SelectList(db.MOVIMIENTO, "ncode_movi", "smone_movi", mOVI_DETALLE.ncode_movi);
            return View(mOVI_DETALLE);
        }

        // GET: MOVI_DETALLE/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVI_DETALLE mOVI_DETALLE = await db.MOVI_DETALLE.FindAsync(id);
            if (mOVI_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(mOVI_DETALLE);
        }

        // POST: MOVI_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MOVI_DETALLE mOVI_DETALLE = await db.MOVI_DETALLE.FindAsync(id);
            db.MOVI_DETALLE.Remove(mOVI_DETALLE);
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
