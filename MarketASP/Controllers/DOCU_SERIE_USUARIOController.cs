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
    public class DOCU_SERIE_USUARIOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: DOCU_SERIE_USUARIO
        public async Task<ActionResult> Index()
        {
            var dOCU_SERIE_USUARIO = db.DOCU_SERIE_USUARIO.Include(d => d.DOCU_SERIE);
            return View(await dOCU_SERIE_USUARIO.ToListAsync());
        }

        // GET: DOCU_SERIE_USUARIO/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO = await db.DOCU_SERIE_USUARIO.FindAsync(id);
            if (dOCU_SERIE_USUARIO == null)
            {
                return HttpNotFound();
            }
            return View(dOCU_SERIE_USUARIO);
        }

        public ActionResult Create()
        {
            ViewBag.ncode_dose = new SelectList(db.DOCU_SERIE, "ncode_dose", "snumeracion_dose");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_dose,susuario_dose,suser_dose,dfech_dose,susmo_dose,dfemo_dose,ncode_docuus")] DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO)
        {
            if (ModelState.IsValid)
            {
                db.DOCU_SERIE_USUARIO.Add(dOCU_SERIE_USUARIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_dose = new SelectList(db.DOCU_SERIE, "ncode_dose", "snumeracion_dose", dOCU_SERIE_USUARIO.ncode_dose);
            return View(dOCU_SERIE_USUARIO);
        }

        // GET: DOCU_SERIE_USUARIO/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO = await db.DOCU_SERIE_USUARIO.FindAsync(id);
            if (dOCU_SERIE_USUARIO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_dose = new SelectList(db.DOCU_SERIE, "ncode_dose", "snumeracion_dose", dOCU_SERIE_USUARIO.ncode_dose);
            return View(dOCU_SERIE_USUARIO);
        }

        // POST: DOCU_SERIE_USUARIO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_dose,susuario_dose,suser_dose,dfech_dose,susmo_dose,dfemo_dose,ncode_docuus")] DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dOCU_SERIE_USUARIO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_dose = new SelectList(db.DOCU_SERIE, "ncode_dose", "snumeracion_dose", dOCU_SERIE_USUARIO.ncode_dose);
            return View(dOCU_SERIE_USUARIO);
        }

        // GET: DOCU_SERIE_USUARIO/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO = await db.DOCU_SERIE_USUARIO.FindAsync(id);
            if (dOCU_SERIE_USUARIO == null)
            {
                return HttpNotFound();
            }
            return View(dOCU_SERIE_USUARIO);
        }

        // POST: DOCU_SERIE_USUARIO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO = await db.DOCU_SERIE_USUARIO.FindAsync(id);
            db.DOCU_SERIE_USUARIO.Remove(dOCU_SERIE_USUARIO);
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
