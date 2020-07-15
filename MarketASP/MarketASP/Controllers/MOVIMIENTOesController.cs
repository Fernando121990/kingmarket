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
    public class MOVIMIENTOesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: MOVIMIENTOes
        public async Task<ActionResult> Index()
        {
            var mOVIMIENTO = db.MOVIMIENTO.Include(m => m.ALMACEN).Include(m => m.ALMACEN1).Include(m => m.TIPO_MOVIMIENTO);
            return View(await mOVIMIENTO.ToListAsync());
        }

        // GET: MOVIMIENTOes/Details/5
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

        // GET: MOVIMIENTOes/Create
        public ActionResult Create()
        {
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma");
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma");
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO, "ncode_timovi", "sdesc_timovi");
            return View();
        }

        // POST: MOVIMIENTOes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_movi,dfemov_movi,smone_movi,ntc_movi,sobse_movi,besta_movi,sserie_movi,snume_movi,suser_movi,dfech_movi,susmo_movi,dfemo_movi,ncode_timovi,ncode_alma,ndestino_alma,stipo_movi")] MOVIMIENTO mOVIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.MOVIMIENTO.Add(mOVIMIENTO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ndestino_alma);
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO, "ncode_timovi", "sdesc_timovi", mOVIMIENTO.ncode_timovi);
            return View(mOVIMIENTO);
        }

        // GET: MOVIMIENTOes/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ndestino_alma);
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO, "ncode_timovi", "sdesc_timovi", mOVIMIENTO.ncode_timovi);
            return View(mOVIMIENTO);
        }

        // POST: MOVIMIENTOes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_movi,dfemov_movi,smone_movi,ntc_movi,sobse_movi,besta_movi,sserie_movi,snume_movi,suser_movi,dfech_movi,susmo_movi,dfemo_movi,ncode_timovi,ncode_alma,ndestino_alma,stipo_movi")] MOVIMIENTO mOVIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mOVIMIENTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ndestino_alma);
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO, "ncode_timovi", "sdesc_timovi", mOVIMIENTO.ncode_timovi);
            return View(mOVIMIENTO);
        }

        // GET: MOVIMIENTOes/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: MOVIMIENTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            db.MOVIMIENTO.Remove(mOVIMIENTO);
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
