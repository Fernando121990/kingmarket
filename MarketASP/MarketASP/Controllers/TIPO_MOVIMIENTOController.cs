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
    public class TIPO_MOVIMIENTOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: TIPO_MOVIMIENTO
        public async Task<ActionResult> Index()
        {
            return View(await db.TIPO_MOVIMIENTO.ToListAsync());
        }

        // GET: TIPO_MOVIMIENTO/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_MOVIMIENTO tIPO_MOVIMIENTO = await db.TIPO_MOVIMIENTO.FindAsync(id);
            if (tIPO_MOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_MOVIMIENTO);
        }

        // GET: TIPO_MOVIMIENTO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TIPO_MOVIMIENTO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_timovi,sdesc_timovi,binout_tipomovi,btransf_tipomovi,besta_tipomovi")] TIPO_MOVIMIENTO tIPO_MOVIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.TIPO_MOVIMIENTO.Add(tIPO_MOVIMIENTO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tIPO_MOVIMIENTO);
        }

        // GET: TIPO_MOVIMIENTO/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_MOVIMIENTO tIPO_MOVIMIENTO = await db.TIPO_MOVIMIENTO.FindAsync(id);
            if (tIPO_MOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_MOVIMIENTO);
        }

        // POST: TIPO_MOVIMIENTO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_timovi,sdesc_timovi,binout_tipomovi,btransf_tipomovi,besta_tipomovi")] TIPO_MOVIMIENTO tIPO_MOVIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIPO_MOVIMIENTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tIPO_MOVIMIENTO);
        }

        // GET: TIPO_MOVIMIENTO/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_MOVIMIENTO tIPO_MOVIMIENTO = await db.TIPO_MOVIMIENTO.FindAsync(id);
            if (tIPO_MOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_MOVIMIENTO);
        }

        // POST: TIPO_MOVIMIENTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TIPO_MOVIMIENTO tIPO_MOVIMIENTO = await db.TIPO_MOVIMIENTO.FindAsync(id);
            db.TIPO_MOVIMIENTO.Remove(tIPO_MOVIMIENTO);
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
