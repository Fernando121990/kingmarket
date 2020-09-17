using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarketASP.Areas.Administracion.Models;

namespace MarketASP.Areas.Administracion.Controllers
{
    public class LOCALsController : Controller
    {
        private MarketWebEntitiesAdmin db = new MarketWebEntitiesAdmin();

        public async Task<ActionResult> Index()
        {
            var lOCAL = db.LOCAL.Include(l => l.SUCURSAL);
            return View(await lOCAL.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCAL lOCAL = await db.LOCAL.FindAsync(id);
            if (lOCAL == null)
            {
                return HttpNotFound();
            }
            return View(lOCAL);
        }

        public ActionResult Create()
        {
            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s =>s.bacti_sucu==true).OrderByDescending(s=>s.sdesc_sucu), "ncode_sucu", "sdesc_sucu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_local,sdesc_local,bacti_local,ncode_sucu")] LOCAL lOCAL)
        {
            if (ModelState.IsValid)
            {
                db.LOCAL.Add(lOCAL);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s => s.bacti_sucu == true).OrderByDescending(s => s.sdesc_sucu), "ncode_sucu", "sdesc_sucu", lOCAL.ncode_sucu);
            return View(lOCAL);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCAL lOCAL = await db.LOCAL.FindAsync(id);
            if (lOCAL == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s => s.bacti_sucu == true).OrderByDescending(s => s.sdesc_sucu), "ncode_sucu", "sdesc_sucu", lOCAL.ncode_sucu);
            return View(lOCAL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_local,sdesc_local,bacti_local,ncode_sucu")] LOCAL lOCAL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOCAL).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s => s.bacti_sucu == true).OrderByDescending(s => s.sdesc_sucu), "ncode_sucu", "sdesc_sucu", lOCAL.ncode_sucu);
            return View(lOCAL);
        }

        public async Task<ActionResult> DeleteLocal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LOCAL lOCAL  = await db.LOCAL.FindAsync(id);
            if (lOCAL == null)
            {
                return HttpNotFound();
            }
            db.LOCAL.Remove(lOCAL);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LOCAL lOCAL = await db.LOCAL.FindAsync(id);
        //    if (lOCAL == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(lOCAL);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    LOCAL lOCAL = await db.LOCAL.FindAsync(id);
        //    db.LOCAL.Remove(lOCAL);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
