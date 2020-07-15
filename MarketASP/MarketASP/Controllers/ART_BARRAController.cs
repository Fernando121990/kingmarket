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
    public class ART_BARRAController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ART_BARRA
        public async Task<ActionResult> Index()
        {
            var aRT_BARRA = db.ART_BARRA.Include(a => a.ARTICULO);
            return View(await aRT_BARRA.ToListAsync());
        }

        // GET: ART_BARRA/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_BARRA aRT_BARRA = await db.ART_BARRA.FindAsync(id);
            if (aRT_BARRA == null)
            {
                return HttpNotFound();
            }
            return View(aRT_BARRA);
        }

        // GET: ART_BARRA/Create
        public ActionResult Create()
        {
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            return View();
        }

        // POST: ART_BARRA/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_barra,sdesc_barra,nesta_barra,ncode_arti,suser_barra,dfech_barra,susmo_barra,dfemo_barra")] ART_BARRA aRT_BARRA)
        {
            if (ModelState.IsValid)
            {
                db.ART_BARRA.Add(aRT_BARRA);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_BARRA.ncode_arti);
            return View(aRT_BARRA);
        }

        // GET: ART_BARRA/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_BARRA aRT_BARRA = await db.ART_BARRA.FindAsync(id);
            if (aRT_BARRA == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_BARRA.ncode_arti);
            return View(aRT_BARRA);
        }

        // POST: ART_BARRA/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_barra,sdesc_barra,nesta_barra,ncode_arti,suser_barra,dfech_barra,susmo_barra,dfemo_barra")] ART_BARRA aRT_BARRA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRT_BARRA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_BARRA.ncode_arti);
            return View(aRT_BARRA);
        }

        // GET: ART_BARRA/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_BARRA aRT_BARRA = await db.ART_BARRA.FindAsync(id);
            if (aRT_BARRA == null)
            {
                return HttpNotFound();
            }
            return View(aRT_BARRA);
        }

        // POST: ART_BARRA/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ART_BARRA aRT_BARRA = await db.ART_BARRA.FindAsync(id);
            db.ART_BARRA.Remove(aRT_BARRA);
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
