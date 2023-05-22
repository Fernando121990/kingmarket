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
    public class EspecieController : Controller
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

            return View(await db.ESPECIE.ToListAsync());
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
        public async Task<ActionResult> Create(ESPECIE especie)
        {
            if (ModelState.IsValid)
            {
                especie.nesta_espe = true;
                especie.suser_espe = User.Identity.Name;
                especie.dfech_espe = DateTime.Now;
                db.ESPECIE.Add(especie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(especie);
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
            ESPECIE eSPECIE = await db.ESPECIE.FindAsync(id);
            if (eSPECIE == null)
            {
                return HttpNotFound();
            }
            return View(eSPECIE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ESPECIE eSPECIE)
        {
            if (ModelState.IsValid)
            {
                eSPECIE.susmo_espe = User.Identity.Name;
                eSPECIE.dfemo_espe = DateTime.Now;

                db.Entry(eSPECIE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(eSPECIE);
        }

        public async Task<ActionResult> DeleteEspe(int? id)
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

            ESPECIE eSPECIE = await db.ESPECIE.FindAsync(id);
            db.ESPECIE.Remove(eSPECIE);
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