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
    public class SublineaController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public ActionResult Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0105", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(db.Pr_Sublinealista().ToList());
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0106", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }
            ViewBag.ncode_linea = new SelectList(db.LINEA.Where(x => x.nesta_linea == true), "ncode_linea", "sdesc_linea");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SUBLINEA sublinea)
        {
            if (ModelState.IsValid)
            {
                db.SUBLINEA.Add(sublinea);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_linea = new SelectList(db.LINEA.Where(x => x.nesta_linea == true), "ncode_linea", "sdesc_linea");
            return View(sublinea);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0107", xcode);
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
            SUBLINEA sublinea = await db.SUBLINEA.FindAsync(id);
            if (sublinea == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_linea = new SelectList(db.LINEA.Where(x => x.nesta_linea == true), "ncode_linea", "sdesc_linea",sublinea.ncode_linea);
            return View(sublinea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SUBLINEA sublinea)
        {
            if (ModelState.IsValid)
            {

                db.Entry(sublinea).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_linea = new SelectList(db.LINEA.Where(x => x.nesta_linea == true), "ncode_linea", "sdesc_linea", sublinea.ncode_linea);
            return View(sublinea);
        }

        public async Task<ActionResult> Deletesublinea(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0108", xcode);
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

            SUBLINEA sublinea = await db.SUBLINEA.FindAsync(id);
            db.SUBLINEA.Remove(sublinea);
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