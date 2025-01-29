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
    public class PROV_CONTACTOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: PROV_CONTACTO
        public async Task<ActionResult> Index()
        {
            var pROV_CONTACTO = db.PROV_CONTACTO.Include(p => p.CONTACTO).Include(p => p.PROVEEDOR);
            return View(await pROV_CONTACTO.ToListAsync());
        }

        // GET: PROV_CONTACTO/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROV_CONTACTO pROV_CONTACTO = await db.PROV_CONTACTO.FindAsync(id);
            if (pROV_CONTACTO == null)
            {
                return HttpNotFound();
            }
            return View(pROV_CONTACTO);
        }

        // GET: PROV_CONTACTO/Create
        public ActionResult Create()
        {
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac");
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove");
            return View();
        }

        // POST: PROV_CONTACTO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_provcontac,ncode_provee,ncode_contac,besta_provcontac")] PROV_CONTACTO pROV_CONTACTO)
        {
            if (ModelState.IsValid)
            {
                db.PROV_CONTACTO.Add(pROV_CONTACTO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac", pROV_CONTACTO.ncode_contac);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", pROV_CONTACTO.ncode_provee);
            return View(pROV_CONTACTO);
        }

        // GET: PROV_CONTACTO/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROV_CONTACTO pROV_CONTACTO = await db.PROV_CONTACTO.FindAsync(id);
            if (pROV_CONTACTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac", pROV_CONTACTO.ncode_contac);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", pROV_CONTACTO.ncode_provee);
            return View(pROV_CONTACTO);
        }

        // POST: PROV_CONTACTO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_provcontac,ncode_provee,ncode_contac,besta_provcontac")] PROV_CONTACTO pROV_CONTACTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pROV_CONTACTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_contac = new SelectList(db.CONTACTO, "ncode_contac", "snomb_contac", pROV_CONTACTO.ncode_contac);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", pROV_CONTACTO.ncode_provee);
            return View(pROV_CONTACTO);
        }

        // GET: PROV_CONTACTO/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROV_CONTACTO pROV_CONTACTO = await db.PROV_CONTACTO.FindAsync(id);
            if (pROV_CONTACTO == null)
            {
                return HttpNotFound();
            }
            return View(pROV_CONTACTO);
        }

        // POST: PROV_CONTACTO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PROV_CONTACTO pROV_CONTACTO = await db.PROV_CONTACTO.FindAsync(id);
            db.PROV_CONTACTO.Remove(pROV_CONTACTO);
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
