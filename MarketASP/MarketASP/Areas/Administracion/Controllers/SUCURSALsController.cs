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
    public class SUCURSALsController : Controller
    {
        private MarketWebEntitiesAdmin db = new MarketWebEntitiesAdmin();

        // GET: Administracion/SUCURSALs
        public async Task<ActionResult> Index()
        {
            return View(await db.SUCURSAL.ToListAsync());
        }

        // GET: Administracion/SUCURSALs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUCURSAL sUCURSAL = await db.SUCURSAL.FindAsync(id);
            if (sUCURSAL == null)
            {
                return HttpNotFound();
            }
            return View(sUCURSAL);
        }

        // GET: Administracion/SUCURSALs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administracion/SUCURSALs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_sucu,sdesc_sucu,bacti_sucu")] SUCURSAL sUCURSAL)
        {
            if (ModelState.IsValid)
            {
                db.SUCURSAL.Add(sUCURSAL);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sUCURSAL);
        }

        // GET: Administracion/SUCURSALs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SUCURSAL sUCURSAL = await db.SUCURSAL.FindAsync(id);
            if (sUCURSAL == null)
            {
                return HttpNotFound();
            }
            return View(sUCURSAL);
        }

        // POST: Administracion/SUCURSALs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_sucu,sdesc_sucu,bacti_sucu")] SUCURSAL sUCURSAL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sUCURSAL).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sUCURSAL);
        }
        public async Task<ActionResult> DeleteSucu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SUCURSAL sUCURSAL  = await db.SUCURSAL.FindAsync(id);
            if (sUCURSAL == null)
            {
                return HttpNotFound();
            }
            db.SUCURSAL.Remove(sUCURSAL);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Administracion/SUCURSALs/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SUCURSAL sUCURSAL = await db.SUCURSAL.FindAsync(id);
        //    if (sUCURSAL == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sUCURSAL);
        //}

        //// POST: Administracion/SUCURSALs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    SUCURSAL sUCURSAL = await db.SUCURSAL.FindAsync(id);
        //    db.SUCURSAL.Remove(sUCURSAL);
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
