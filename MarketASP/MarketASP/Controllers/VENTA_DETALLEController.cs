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
    public class VENTA_DETALLEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: VENTA_DETALLE
        public async Task<ActionResult> Index()
        {
            var vENTA_DETALLE = db.VENTA_DETALLE.Include(v => v.ALMACEN).Include(v => v.ARTICULO).Include(v => v.VENTAS);
            return View(await vENTA_DETALLE.ToListAsync());
        }

        // GET: VENTA_DETALLE/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTA_DETALLE vENTA_DETALLE = await db.VENTA_DETALLE.FindAsync(id);
            if (vENTA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(vENTA_DETALLE);
        }

        // GET: VENTA_DETALLE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma");
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti");
            ViewBag.ncode_venta = new SelectList(db.VENTAS, "ncode_venta", "sseri_venta");
            return View();
        }

        // POST: VENTA_DETALLE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_vedeta,ncode_venta,ncode_arti,ncant_vedeta,npu_vedeta,ndscto_vedeta,ndscto2_vedeta,nexon_vedeta,nafecto_vedeta,besafecto_vedeta,ncode_alma,ndsctomax_vedeta,ndsctomin_vedeta,ndsctoporc_vedeta")] VENTA_DETALLE vENTA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.VENTA_DETALLE.Add(vENTA_DETALLE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", vENTA_DETALLE.ncode_alma);
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", vENTA_DETALLE.ncode_arti);
            ViewBag.ncode_venta = new SelectList(db.VENTAS, "ncode_venta", "sseri_venta", vENTA_DETALLE.ncode_venta);
            return View(vENTA_DETALLE);
        }

        // GET: VENTA_DETALLE/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTA_DETALLE vENTA_DETALLE = await db.VENTA_DETALLE.FindAsync(id);
            if (vENTA_DETALLE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", vENTA_DETALLE.ncode_alma);
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", vENTA_DETALLE.ncode_arti);
            ViewBag.ncode_venta = new SelectList(db.VENTAS, "ncode_venta", "sseri_venta", vENTA_DETALLE.ncode_venta);
            return View(vENTA_DETALLE);
        }

        // POST: VENTA_DETALLE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_vedeta,ncode_venta,ncode_arti,ncant_vedeta,npu_vedeta,ndscto_vedeta,ndscto2_vedeta,nexon_vedeta,nafecto_vedeta,besafecto_vedeta,ncode_alma,ndsctomax_vedeta,ndsctomin_vedeta,ndsctoporc_vedeta")] VENTA_DETALLE vENTA_DETALLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vENTA_DETALLE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", vENTA_DETALLE.ncode_alma);
            ViewBag.ncode_arti = new SelectList(db.ARTICULO, "ncode_arti", "sdesc1_arti", vENTA_DETALLE.ncode_arti);
            ViewBag.ncode_venta = new SelectList(db.VENTAS, "ncode_venta", "sseri_venta", vENTA_DETALLE.ncode_venta);
            return View(vENTA_DETALLE);
        }

        // GET: VENTA_DETALLE/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTA_DETALLE vENTA_DETALLE = await db.VENTA_DETALLE.FindAsync(id);
            if (vENTA_DETALLE == null)
            {
                return HttpNotFound();
            }
            return View(vENTA_DETALLE);
        }

        // POST: VENTA_DETALLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            VENTA_DETALLE vENTA_DETALLE = await db.VENTA_DETALLE.FindAsync(id);
            db.VENTA_DETALLE.Remove(vENTA_DETALLE);
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
