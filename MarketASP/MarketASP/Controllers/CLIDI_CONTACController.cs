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
    public class CLIDI_CONTACController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLIDI_CONTAC
        public async Task<ActionResult> Index()
        {
            var cLIDI_CONTAC = db.CLIDI_CONTAC.Include(c => c.CLI_DIRE).Include(c => c.CONTACTO);
            return View(await cLIDI_CONTAC.ToListAsync());
        }

        // GET: CLIDI_CONTAC/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIDI_CONTAC cLIDI_CONTAC = await db.CLIDI_CONTAC.FindAsync(id);
            if (cLIDI_CONTAC == null)
            {
                return HttpNotFound();
            }
            return View(cLIDI_CONTAC);
        }

        // GET: CLIDI_CONTAC/Create
        public ActionResult Create()
        {
            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire");
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac");
            return View();
        }

        // POST: CLIDI_CONTAC/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_clidco,ncode_clidire,ncode_contac,besta_clidco")] CLIDI_CONTAC cLIDI_CONTAC)
        {
            if (ModelState.IsValid)
            {
                db.CLIDI_CONTAC.Add(cLIDI_CONTAC);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire", cLIDI_CONTAC.ncode_clidire);
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac", cLIDI_CONTAC.ncode_contac);
            return View(cLIDI_CONTAC);
        }

        // GET: CLIDI_CONTAC/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIDI_CONTAC cLIDI_CONTAC = await db.CLIDI_CONTAC.FindAsync(id);
            if (cLIDI_CONTAC == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire", cLIDI_CONTAC.ncode_clidire);
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac", cLIDI_CONTAC.ncode_contac);
            return View(cLIDI_CONTAC);
        }

        // POST: CLIDI_CONTAC/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_clidco,ncode_clidire,ncode_contac,besta_clidco")] CLIDI_CONTAC cLIDI_CONTAC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLIDI_CONTAC).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_clidire = new SelectList(db.CLI_DIRE, "ncode_clidire", "sdesc_clidire", cLIDI_CONTAC.ncode_clidire);
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac", cLIDI_CONTAC.ncode_contac);
            return View(cLIDI_CONTAC);
        }

        // GET: CLIDI_CONTAC/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIDI_CONTAC cLIDI_CONTAC = await db.CLIDI_CONTAC.FindAsync(id);
            if (cLIDI_CONTAC == null)
            {
                return HttpNotFound();
            }
            return View(cLIDI_CONTAC);
        }

        // POST: CLIDI_CONTAC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CLIDI_CONTAC cLIDI_CONTAC = await db.CLIDI_CONTAC.FindAsync(id);
            db.CLIDI_CONTAC.Remove(cLIDI_CONTAC);
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
