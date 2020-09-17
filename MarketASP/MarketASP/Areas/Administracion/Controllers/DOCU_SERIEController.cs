using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MarketASP.Areas.Administracion.Models;

namespace MarketASP.Areas.Administracion.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DOCU_SERIEController : Controller
    {
        private MarketWebEntitiesAdmin db = new MarketWebEntitiesAdmin();

        // GET: Administracion/DOCU_SERIE
        public async Task<ActionResult> Index()
        {
            //var dOCU_SERIE = db.DOCU_SERIE.Include(d => d.CONFIGURACION).Include(d => d.CONFIGURACION1);
            var dOCU_SERIE = db.DOCU_SERIE.Include(d => d.CONFIGURACION).Include(d => d.LOCAL);
            return View(await dOCU_SERIE.ToListAsync());
        }


        public ActionResult Create()
        {
            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L=>L.bacti_local==true), "ncode_local", "sdesc_local");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D=>D.ntipo_confi==5), "ncode_confi", "sdesc_confi");
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_dose,ncode_local,ncode_docu,snumeracion_dose,susuario_dose,sdocumento_dose")] DOCU_SERIE dOCU_SERIE)
        {
            if (ModelState.IsValid)
            {
                dOCU_SERIE.suser_dose = User.Identity.Name;
                dOCU_SERIE.dfech_dose = DateTime.Now;
                db.DOCU_SERIE.Add(dOCU_SERIE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_local = new SelectList(db.CONFIGURACION.Where(L => L.ntipo_confi == 10), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(L => L.ntipo_confi == 5), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_docu);
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName",dOCU_SERIE.susuario_dose);
            return View(dOCU_SERIE);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_local = new SelectList(db.CONFIGURACION.Where(L => L.ntipo_confi == 10), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(L => L.ntipo_confi == 5), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_docu);
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName", dOCU_SERIE.susuario_dose);
            return View(dOCU_SERIE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_dose,ncode_local,ncode_docu,snumeracion_dose,susuario_dose,sdocumento_dose")] DOCU_SERIE dOCU_SERIE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dOCU_SERIE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_local = new SelectList(db.CONFIGURACION.Where(L => L.ntipo_confi == 10), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(L => L.ntipo_confi == 5), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_docu);
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName", dOCU_SERIE.susuario_dose);
            return View(dOCU_SERIE);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }
            return View(dOCU_SERIE);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            db.DOCU_SERIE.Remove(dOCU_SERIE);
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
