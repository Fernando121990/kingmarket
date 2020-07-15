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
    public class ALMACENsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ALMACENs
        public async Task<ActionResult> Index()
        {
            return View(await db.ALMACEN.ToListAsync());
        }

        // GET: ALMACENs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALMACEN aLMACEN = await db.ALMACEN.FindAsync(id);
            if (aLMACEN == null)
            {
                return HttpNotFound();
            }
            return View(aLMACEN);
        }

        // GET: ALMACENs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ALMACENs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_alma,sdesc_alma,besta_alma")] ALMACEN aLMACEN)
        {
            if (ModelState.IsValid)
            {
                db.ALMACEN.Add(aLMACEN);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(aLMACEN);
        }

        // GET: ALMACENs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALMACEN aLMACEN = await db.ALMACEN.FindAsync(id);
            if (aLMACEN == null)
            {
                return HttpNotFound();
            }
            return View(aLMACEN);
        }

        // POST: ALMACENs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_alma,sdesc_alma,besta_alma")] ALMACEN aLMACEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aLMACEN).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aLMACEN);
        }

        // GET: ALMACENs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ALMACEN aLMACEN = await db.ALMACEN.FindAsync(id);
            if (aLMACEN == null)
            {
                return HttpNotFound();
            }
            return View(aLMACEN);
        }

        // POST: ALMACENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ALMACEN aLMACEN = await db.ALMACEN.FindAsync(id);
            db.ALMACEN.Remove(aLMACEN);
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
