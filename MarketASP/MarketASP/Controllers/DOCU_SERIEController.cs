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
using System.Data.Entity.Core.Objects;

namespace MarketASP.Controllers
{
    [Authorize]
    public class DOCU_SERIEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Administracion/DOCU_SERIE
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1005", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            //var dOCU_SERIE = db.DOCU_SERIE.Include(d => d.CONFIGURACION).Include(d => d.CONFIGURACION1);
            var dOCU_SERIE = db.DOCU_SERIE.Include(d => d.CONFIGURACION).Include(d => d.LOCAL);
            return View(await dOCU_SERIE.ToListAsync());
        }


        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1006", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L=>L.bacti_local==true), "ncode_local", "sdesc_local");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D=>D.ntipo_confi==5), "ncode_confi", "sdesc_confi");
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DOCU_SERIE dOCU_SERIE)
        {
            if (ModelState.IsValid)
            {
                dOCU_SERIE.suser_dose = User.Identity.Name;
                dOCU_SERIE.dfech_dose = DateTime.Now;
                db.DOCU_SERIE.Add(dOCU_SERIE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L => L.bacti_local == true), "ncode_local", "sdesc_local");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D => D.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName");
            return View(dOCU_SERIE);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1007", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L => L.bacti_local == true), "ncode_local", "sdesc_local",dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D => D.ntipo_confi == 5), "ncode_confi", "sdesc_confi",dOCU_SERIE.ncode_docu);
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName",dOCU_SERIE.susuario_dose);

            return View(dOCU_SERIE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DOCU_SERIE dOCU_SERIE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dOCU_SERIE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L => L.bacti_local == true), "ncode_local", "sdesc_local", dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D => D.ntipo_confi == 5), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_docu);
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName", dOCU_SERIE.susuario_dose);

            return View(dOCU_SERIE);
        }

        public async Task<ActionResult> DeleteDocSerie(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1008", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }
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
