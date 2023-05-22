using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    public class TransportistaController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0701", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(await db.TRANSPORTISTA.ToListAsync());
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0702", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TRANSPORTISTA tRANSPORTISTA)
        {
            if (ModelState.IsValid)
            {
                tRANSPORTISTA.nesta_tran = true;
                tRANSPORTISTA.suser_tran = User.Identity.Name;
                tRANSPORTISTA.dfech_tran = DateTime.Now;
                db.TRANSPORTISTA.Add(tRANSPORTISTA);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tRANSPORTISTA);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0703", xcode);
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
            TRANSPORTISTA tRANSPORTISTA = await db.TRANSPORTISTA.FindAsync(id);
            if (tRANSPORTISTA == null)
            {
                return HttpNotFound();
            }
            return View(tRANSPORTISTA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TRANSPORTISTA tRANSPORTISTA)
        {
            if (ModelState.IsValid)
            {
                tRANSPORTISTA.susmo_tran = User.Identity.Name;
                tRANSPORTISTA.dfemo_tran = DateTime.Now;

                db.Entry(tRANSPORTISTA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tRANSPORTISTA);
        }

        public async Task<ActionResult> Deletetran(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0704", xcode);
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

            TRANSPORTISTA tRANSPORTISTA = await db.TRANSPORTISTA.FindAsync(id);
            db.TRANSPORTISTA.Remove(tRANSPORTISTA);
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