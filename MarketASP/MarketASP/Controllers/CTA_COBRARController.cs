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
    public class CTA_COBRARController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CTA_COBRAR
        public async Task<ActionResult> Index()
        {
            var cTA_COBRAR = db.CTA_COBRAR.Include(c => c.CLIENTE).Include(c => c.CONFIGURACION);
            return View(await cTA_COBRAR.ToListAsync());
        }

        // GET: CTA_COBRAR/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
            if (cTA_COBRAR == null)
            {
                return HttpNotFound();
            }
            return View(cTA_COBRAR);
        }

        // GET: CTA_COBRAR/Create
        public ActionResult Create()
        {
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            return View();
        }

        // POST: CTA_COBRAR/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_ctaco,ncodeDoc_ctaco,sdocu_ctaco,dfecta_ctaco,smone_ctaco,ntotal_ctaco,dfevenci_ctaco,ntc_ctaco,ntotalMN_ctaco,ntotalUS_ctaco,npago_ctaco,ncode_letra,sesta_letra,suser_ctaco,dfech_ctaco,susmo_ctaco,dfemo_ctaco,ncode_cliente,ncode_docu")] CTA_COBRAR cTA_COBRAR)
        {
            if (ModelState.IsValid)
            {
                db.CTA_COBRAR.Add(cTA_COBRAR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cTA_COBRAR.ncode_cliente);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTA_COBRAR.ncode_docu);
            return View(cTA_COBRAR);
        }

        // GET: CTA_COBRAR/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
            if (cTA_COBRAR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cTA_COBRAR.ncode_cliente);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTA_COBRAR.ncode_docu);
            return View(cTA_COBRAR);
        }

        // POST: CTA_COBRAR/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_ctaco,ncodeDoc_ctaco,sdocu_ctaco,dfecta_ctaco,smone_ctaco,ntotal_ctaco,dfevenci_ctaco,ntc_ctaco,ntotalMN_ctaco,ntotalUS_ctaco,npago_ctaco,ncode_letra,sesta_letra,suser_ctaco,dfech_ctaco,susmo_ctaco,dfemo_ctaco,ncode_cliente,ncode_docu")] CTA_COBRAR cTA_COBRAR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTA_COBRAR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cTA_COBRAR.ncode_cliente);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTA_COBRAR.ncode_docu);
            return View(cTA_COBRAR);
        }

        // GET: CTA_COBRAR/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
            if (cTA_COBRAR == null)
            {
                return HttpNotFound();
            }
            return View(cTA_COBRAR);
        }

        // POST: CTA_COBRAR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
            db.CTA_COBRAR.Remove(cTA_COBRAR);
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
