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

namespace MarketASP.Areas.Inventario.Controllers
{
    public class ALMACENsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ALMACENs
        public async Task<ActionResult> Index()
        {
            return View(await db.ALMACEN.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

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

        public async Task<ActionResult> DeleteAlma(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

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
