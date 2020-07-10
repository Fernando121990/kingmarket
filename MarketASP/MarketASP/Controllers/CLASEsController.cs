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
    public class CLASEsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLASEs
        public async Task<ActionResult> Index()
        {
            var cLASE = db.CLASE.Include(c => c.FAMILIA);
            return View(await cLASE.ToListAsync());
        }

        // GET: CLASEs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASE cLASE = await db.CLASE.FindAsync(id);
            if (cLASE == null)
            {
                return HttpNotFound();
            }
            return View(cLASE);
        }

        // GET: CLASEs/Create
        public ActionResult Create()
        {
            ViewBag.ncode_fami = new SelectList(db.FAMILIA, "ncode_fami", "sdesc_fami");
            return View();
        }

        // POST: CLASEs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_clase,sdesc_clase,nesta_clase,ncode_fami,suser_clase,dfech_clase,susmo_clase,dfemo_clase")] CLASE cLASE)
        {
            if (ModelState.IsValid)
            {
                db.CLASE.Add(cLASE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_fami = new SelectList(db.FAMILIA, "ncode_fami", "sdesc_fami", cLASE.ncode_fami);
            return View(cLASE);
        }

        // GET: CLASEs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASE cLASE = await db.CLASE.FindAsync(id);
            if (cLASE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA, "ncode_fami", "sdesc_fami", cLASE.ncode_fami);
            return View(cLASE);
        }

        // POST: CLASEs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_clase,sdesc_clase,nesta_clase,ncode_fami,suser_clase,dfech_clase,susmo_clase,dfemo_clase")] CLASE cLASE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLASE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA, "ncode_fami", "sdesc_fami", cLASE.ncode_fami);
            return View(cLASE);
        }

        // GET: CLASEs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASE cLASE = await db.CLASE.FindAsync(id);
            if (cLASE == null)
            {
                return HttpNotFound();
            }
            return View(cLASE);
        }

        // POST: CLASEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CLASE cLASE = await db.CLASE.FindAsync(id);
            db.CLASE.Remove(cLASE);
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
