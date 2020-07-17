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
    public class KARDEXesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: KARDEXes
        public async Task<ActionResult> Index()
        {
            var kARDEX = db.KARDEX.Include(k => k.ALMACEN);
            return View(await kARDEX.ToListAsync());
        }

        // GET: KARDEXes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            return View(kARDEX);
        }

        // GET: KARDEXes/Create
        public ActionResult Create()
        {
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma");
            return View();
        }

        // POST: KARDEXes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_kardex,dfekard_kardex,stipomovi_kardex,ncant_kardex,npuco_kardex,ntc_kardex,smone_kardex,npucoMN_kardex,npucoUS_kardex,ncodeDoc_kardex,sserie_kardex,snume_kardex,dfvence_kardex,suser_kardex,dfech_kardex,susmo_kardex,dfemo_kardex,ncode_alma")] KARDEX kARDEX)
        {
            if (ModelState.IsValid)
            {
                db.KARDEX.Add(kARDEX);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        // GET: KARDEXes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        // POST: KARDEXes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_kardex,dfekard_kardex,stipomovi_kardex,ncant_kardex,npuco_kardex,ntc_kardex,smone_kardex,npucoMN_kardex,npucoUS_kardex,ncodeDoc_kardex,sserie_kardex,snume_kardex,dfvence_kardex,suser_kardex,dfech_kardex,susmo_kardex,dfemo_kardex,ncode_alma")] KARDEX kARDEX)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kARDEX).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        // GET: KARDEXes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            return View(kARDEX);
        }

        // POST: KARDEXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            db.KARDEX.Remove(kARDEX);
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
