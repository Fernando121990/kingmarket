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
    public class CONFIGURACIONsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CONFIGURACIONs
        public async Task<ActionResult> Index()
        {
            return View(await db.CONFIGURACION.ToListAsync());
        }

        // GET: CONFIGURACIONs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            if (cONFIGURACION == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION);
        }

        // GET: CONFIGURACIONs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CONFIGURACIONs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_confi,sdesc_confi,svalor_confi,besta_confi,ntipo_confi")] CONFIGURACION cONFIGURACION)
        {
            if (ModelState.IsValid)
            {
                db.CONFIGURACION.Add(cONFIGURACION);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cONFIGURACION);
        }

        // GET: CONFIGURACIONs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            if (cONFIGURACION == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION);
        }

        // POST: CONFIGURACIONs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_confi,sdesc_confi,svalor_confi,besta_confi,ntipo_confi")] CONFIGURACION cONFIGURACION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cONFIGURACION).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cONFIGURACION);
        }

        // GET: CONFIGURACIONs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            if (cONFIGURACION == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION);
        }

        // POST: CONFIGURACIONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            db.CONFIGURACION.Remove(cONFIGURACION);
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
