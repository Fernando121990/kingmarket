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
    public class ART_PROVEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ART_PROVE
        public async Task<ActionResult> Index()
        {
            var aRT_PROVE = db.ART_PROVE.Include(a => a.ARTICULO).Include(a => a.PROVEEDOR);
            return View(await aRT_PROVE.ToListAsync());
        }

        // GET: ART_PROVE/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PROVE aRT_PROVE = await db.ART_PROVE.FindAsync(id);
            if (aRT_PROVE == null)
            {
                return HttpNotFound();
            }
            return View(aRT_PROVE);
        }

        // GET: ART_PROVE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove");
            return View();
        }

        // POST: ART_PROVE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_arprove,ncode_provee,ncode_arti,suser_arprove,dfech_arprove,susmo_arprove,dfemo_arprove")] ART_PROVE aRT_PROVE)
        {
            if (ModelState.IsValid)
            {
                db.ART_PROVE.Add(aRT_PROVE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_PROVE.ncode_arti);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", aRT_PROVE.ncode_provee);
            return View(aRT_PROVE);
        }

        // GET: ART_PROVE/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PROVE aRT_PROVE = await db.ART_PROVE.FindAsync(id);
            if (aRT_PROVE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_PROVE.ncode_arti);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", aRT_PROVE.ncode_provee);
            return View(aRT_PROVE);
        }

        // POST: ART_PROVE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_arprove,ncode_provee,ncode_arti,suser_arprove,dfech_arprove,susmo_arprove,dfemo_arprove")] ART_PROVE aRT_PROVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRT_PROVE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_PROVE.ncode_arti);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", aRT_PROVE.ncode_provee);
            return View(aRT_PROVE);
        }

        // GET: ART_PROVE/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PROVE aRT_PROVE = await db.ART_PROVE.FindAsync(id);
            if (aRT_PROVE == null)
            {
                return HttpNotFound();
            }
            return View(aRT_PROVE);
        }

        // POST: ART_PROVE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ART_PROVE aRT_PROVE = await db.ART_PROVE.FindAsync(id);
            db.ART_PROVE.Remove(aRT_PROVE);
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
