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
    public class SubespecieController : Controller
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

            return View(await db.SUBESPECIE.ToListAsync());
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
        public async Task<ActionResult> Create(SUBESPECIE sUBESPECIE)
        {
            if (ModelState.IsValid)
            {
                sUBESPECIE.nesta_subesp = true;
                sUBESPECIE.suser_subesp = User.Identity.Name;
                sUBESPECIE.dfech_subesp = DateTime.Now;
                db.SUBESPECIE.Add(sUBESPECIE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sUBESPECIE);
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
            SUBESPECIE sUBESPECIE = await db.SUBESPECIE.FindAsync(id);
            if (sUBESPECIE == null)
            {
                return HttpNotFound();
            }
            return View(sUBESPECIE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SUBESPECIE sUBESPECIE)
        {
            if (ModelState.IsValid)
            {
                sUBESPECIE.susmo_subesp = User.Identity.Name;
                sUBESPECIE.dfemo_subesp = DateTime.Now;

                db.Entry(sUBESPECIE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sUBESPECIE);
        }

        public async Task<ActionResult> DeleteSubesp(int? id)
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

            SUBESPECIE sUBESPECIE = await db.SUBESPECIE.FindAsync(id);
            db.SUBESPECIE.Remove(sUBESPECIE);
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