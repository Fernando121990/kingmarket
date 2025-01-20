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
    [Authorize]
    public class SubespecieController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public ActionResult Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0121", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(db.Pr_SubespecieLista().ToList());
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0122", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }
            ViewBag.ncode_espe = new SelectList(db.ESPECIE.Where(x => x.nesta_espe == true), "ncode_espe", "sdesc_espe");
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
            ViewBag.ncode_espe = new SelectList(db.ESPECIE.Where(x => x.nesta_espe == true), "ncode_espe", "sdesc_espe");
            return View(sUBESPECIE);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0123", xcode);
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
            ViewBag.ncode_espe = new SelectList(db.ESPECIE.Where(x => x.nesta_espe == true), "ncode_espe", "sdesc_espe",sUBESPECIE.ncode_espe);
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
            ViewBag.ncode_espe = new SelectList(db.ESPECIE.Where(x => x.nesta_espe == true), "ncode_espe", "sdesc_espe", sUBESPECIE.ncode_espe);
            return View(sUBESPECIE);
        }

        public async Task<ActionResult> DeleteSubesp(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0124", xcode);
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