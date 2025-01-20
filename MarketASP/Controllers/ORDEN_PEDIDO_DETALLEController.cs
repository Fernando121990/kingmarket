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
    public class ORDEN_PEDIDO_DETALLEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ORDEN_PEDIDO_DETALLE
        public async Task<ActionResult> Index()
        {
            var ORDEN_PEDIDO_DETALLE = db.ORDEN_PEDIDOS_DETALLE.Include(p => p.ARTICULO).Include(p => p.ORDEN_PEDIDOS);
            return View(await ORDEN_PEDIDO_DETALLE.ToListAsync());
        }

        // GET: ORDEN_PEDIDO_DETALLE/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDEN_PEDIDOS_DETALLE oRDEN_PEDIDO_DETALLE = await db.ORDEN_PEDIDOS_DETALLE.FindAsync(id);
            if (oRDEN_PEDIDO_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(oRDEN_PEDIDO_DETALLE);
        }

        // GET: ORDEN_PEDIDO_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            ViewBag.ncode_prof = new SelectList(db.ORDEN_PEDIDOS, "ncode_prof", "sseri_prof");
            return View();
        }

        // POST: ORDEN_PEDIDO_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( ORDEN_PEDIDOS_DETALLE oRDEN_PEDIDO_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.ORDEN_PEDIDOS_DETALLE.Add(oRDEN_PEDIDO_DETALLE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", oRDEN_PEDIDO_DETALLE.ncode_arti);
            ViewBag.ncode_prof = new SelectList(db.ORDEN_PEDIDOS, "ncode_prof", "sseri_prof", oRDEN_PEDIDO_DETALLE.ncode_orpe);
            return View(oRDEN_PEDIDO_DETALLE);
        }

        // GET: ORDEN_PEDIDO_DETALLE/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDEN_PEDIDOS_DETALLE oRDEN_PEDIDO_DETALLE = await db.ORDEN_PEDIDOS_DETALLE.FindAsync(id);
            if (oRDEN_PEDIDO_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", oRDEN_PEDIDO_DETALLE.ncode_arti);
            ViewBag.ncode_prof = new SelectList(db.ORDEN_PEDIDOS, "ncode_prof", "sseri_prof", oRDEN_PEDIDO_DETALLE.ncode_orpe);
            return View(oRDEN_PEDIDO_DETALLE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( ORDEN_PEDIDOS_DETALLE oRDEN_PEDIDO_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oRDEN_PEDIDO_DETALLE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", oRDEN_PEDIDO_DETALLE.ncode_arti);
            ViewBag.ncode_prof = new SelectList(db.ORDEN_PEDIDOS, "ncode_prof", "sseri_prof", oRDEN_PEDIDO_DETALLE.ncode_orpe);
            return View(oRDEN_PEDIDO_DETALLE);
        }

        // GET: ORDEN_PEDIDO_DETALLE/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDEN_PEDIDOS_DETALLE oRDEN_PEDIDO_DETALLE = await db.ORDEN_PEDIDOS_DETALLE.FindAsync(id);
            if (oRDEN_PEDIDO_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(oRDEN_PEDIDO_DETALLE);
        }

        // POST: ORDEN_PEDIDO_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            ORDEN_PEDIDOS_DETALLE oRDEN_PEDIDO_DETALLE = await db.ORDEN_PEDIDOS_DETALLE.FindAsync(id);
            db.ORDEN_PEDIDOS_DETALLE.Remove(oRDEN_PEDIDO_DETALLE);
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
