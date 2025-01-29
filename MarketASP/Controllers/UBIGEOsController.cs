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
    public class UBIGEOsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: UBIGEOs
        public async Task<ActionResult> Index()
        {
            return View(await db.UBIGEO.ToListAsync());
        }

        // GET: UBIGEOs/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UBIGEO uBIGEO = await db.UBIGEO.FindAsync(id);
            if (uBIGEO == null)
            {
                return HttpNotFound();
            }
            return View(uBIGEO);
        }

        // GET: UBIGEOs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UBIGEOs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "scode_ubigeo,sdepa_ubigeo,sprovi_ubigeo,sdistri_ubigeo")] UBIGEO uBIGEO)
        {
            if (ModelState.IsValid)
            {
                db.UBIGEO.Add(uBIGEO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(uBIGEO);
        }

        // GET: UBIGEOs/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UBIGEO uBIGEO = await db.UBIGEO.FindAsync(id);
            if (uBIGEO == null)
            {
                return HttpNotFound();
            }
            return View(uBIGEO);
        }

        // POST: UBIGEOs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "scode_ubigeo,sdepa_ubigeo,sprovi_ubigeo,sdistri_ubigeo")] UBIGEO uBIGEO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uBIGEO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(uBIGEO);
        }

        // GET: UBIGEOs/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UBIGEO uBIGEO = await db.UBIGEO.FindAsync(id);
            if (uBIGEO == null)
            {
                return HttpNotFound();
            }
            return View(uBIGEO);
        }

        // POST: UBIGEOs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            UBIGEO uBIGEO = await db.UBIGEO.FindAsync(id);
            db.UBIGEO.Remove(uBIGEO);
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
