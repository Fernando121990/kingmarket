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
    public class LOTESController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: LOTES
        public async Task<ActionResult> Index()
        {

            var resultado = db.Pr_KardexLotesMovimientos("", "", "").ToList();

            return View(resultado);
        }

        // GET: LOTES/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOTES lOTES = await db.LOTES.FindAsync(id);
            if (lOTES == null)
            {
                return HttpNotFound();
            }
            return View(lOTES);
        }

        public async Task<ActionResult> Create(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            var result = from s in db.COMPRA_DETALLE
                                    join a in db.ARTICULO on s.ncode_arti equals a.ncode_arti 
                         where s.ncode_compra.Equals(id) 
                         select new { s.ncode_arti, a.sdesc1_arti };

            ViewBag.ncode_arti = new SelectList(result, "ncode_Arti", "sdesc1_arti");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LOTES lOTES)
        {
            if (ModelState.IsValid)
            {
                db.LOTES.Add(lOTES);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lOTES);
        }

        // GET: LOTES/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOTES lOTES = await db.LOTES.FindAsync(id);
            if (lOTES == null)
            {
                return HttpNotFound();
            }
            return View(lOTES);
        }

        // POST: LOTES/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_lote,sdesc_lote,dfvenci_lote,ncode_arti,ncode_compra,ncant_lote,ncantdespachada_lote,ncantrestante_lote,besta_lote,dfech_lote,suser_lote,dfemo_lote,susmo_lote")] LOTES lOTES)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOTES).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lOTES);
        }

        // GET: LOTES/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOTES lOTES = await db.LOTES.FindAsync(id);
            if (lOTES == null)
            {
                return HttpNotFound();
            }
            return View(lOTES);
        }

        // POST: LOTES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            LOTES lOTES = await db.LOTES.FindAsync(id);
            db.LOTES.Remove(lOTES);
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
