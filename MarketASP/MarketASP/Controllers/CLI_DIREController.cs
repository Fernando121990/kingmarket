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
    public class CLI_DIREController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLI_DIRE
        public async Task<ActionResult> Index()
        {
            var cLI_DIRE = db.CLI_DIRE.Include(c => c.CLIENTE).Include(c => c.UBIGEO);
            return View(await cLI_DIRE.ToListAsync());
        }

        // GET: CLI_DIRE/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLI_DIRE cLI_DIRE = await db.CLI_DIRE.FindAsync(id);
            if (cLI_DIRE == null)
            {
                return HttpNotFound();
            }
            return View(cLI_DIRE);
        }

        // GET: CLI_DIRE/Create
        public ActionResult Create()
        {
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente");
            ViewBag.scode_ubigeo = new SelectList(db.UBIGEO, "scode_ubigeo", "sdepa_ubigeo");
            return View();
        }

        // POST: CLI_DIRE/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_clidire,sdesc_clidire,ncode_cliente,scode_ubigeo")] CLI_DIRE cLI_DIRE)
        {
            if (ModelState.IsValid)
            {
                db.CLI_DIRE.Add(cLI_DIRE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cLI_DIRE.ncode_cliente);
            ViewBag.scode_ubigeo = new SelectList(db.UBIGEO, "scode_ubigeo", "sdepa_ubigeo", cLI_DIRE.scode_ubigeo);
            return View(cLI_DIRE);
        }

        // GET: CLI_DIRE/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLI_DIRE cLI_DIRE = await db.CLI_DIRE.FindAsync(id);
            if (cLI_DIRE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cLI_DIRE.ncode_cliente);
            ViewBag.scode_ubigeo = new SelectList(db.UBIGEO, "scode_ubigeo", "sdepa_ubigeo", cLI_DIRE.scode_ubigeo);
            return View(cLI_DIRE);
        }

        // POST: CLI_DIRE/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_clidire,sdesc_clidire,ncode_cliente,scode_ubigeo")] CLI_DIRE cLI_DIRE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLI_DIRE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cLI_DIRE.ncode_cliente);
            ViewBag.scode_ubigeo = new SelectList(db.UBIGEO, "scode_ubigeo", "sdepa_ubigeo", cLI_DIRE.scode_ubigeo);
            return View(cLI_DIRE);
        }

        // GET: CLI_DIRE/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLI_DIRE cLI_DIRE = await db.CLI_DIRE.FindAsync(id);
            if (cLI_DIRE == null)
            {
                return HttpNotFound();
            }
            return View(cLI_DIRE);
        }

        // POST: CLI_DIRE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CLI_DIRE cLI_DIRE = await db.CLI_DIRE.FindAsync(id);
            db.CLI_DIRE.Remove(cLI_DIRE);
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
