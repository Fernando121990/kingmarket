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
    public class ART_PRECIOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ART_PRECIO
        public async Task<ActionResult> Index()
        {
            var aRT_PRECIO = db.ART_PRECIO.Include(a => a.LISTA_PRECIO).Include(a => a.ARTICULO);
            return View(await aRT_PRECIO.ToListAsync());
        }

        // GET: ART_PRECIO/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PRECIO aRT_PRECIO = await db.ART_PRECIO.FindAsync(id);
            if (aRT_PRECIO == null)
            {
                return HttpNotFound();
            }
            return View(aRT_PRECIO);
        }

        // GET: ART_PRECIO/Create
        public ActionResult Create()
        {
            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre");
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            return View();
        }

        // POST: ART_PRECIO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_artpre,nprecio_artpre,nesta_artpre,ncode_lipre,ncode_arti,suser_artpre,dfech_artpre,susmo_artpre,dfemo_artpre")] ART_PRECIO aRT_PRECIO)
        {
            if (ModelState.IsValid)
            {
                db.ART_PRECIO.Add(aRT_PRECIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre", aRT_PRECIO.ncode_lipre);
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_PRECIO.ncode_arti);
            return View(aRT_PRECIO);
        }

        // GET: ART_PRECIO/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PRECIO aRT_PRECIO = await db.ART_PRECIO.FindAsync(id);
            if (aRT_PRECIO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre", aRT_PRECIO.ncode_lipre);
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_PRECIO.ncode_arti);
            return View(aRT_PRECIO);
        }

        // POST: ART_PRECIO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_artpre,nprecio_artpre,nesta_artpre,ncode_lipre,ncode_arti,suser_artpre,dfech_artpre,susmo_artpre,dfemo_artpre")] ART_PRECIO aRT_PRECIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRT_PRECIO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre", aRT_PRECIO.ncode_lipre);
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", aRT_PRECIO.ncode_arti);
            return View(aRT_PRECIO);
        }

        // GET: ART_PRECIO/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PRECIO aRT_PRECIO = await db.ART_PRECIO.FindAsync(id);
            if (aRT_PRECIO == null)
            {
                return HttpNotFound();
            }
            return View(aRT_PRECIO);
        }

        // POST: ART_PRECIO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ART_PRECIO aRT_PRECIO = await db.ART_PRECIO.FindAsync(id);
            db.ART_PRECIO.Remove(aRT_PRECIO);
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
