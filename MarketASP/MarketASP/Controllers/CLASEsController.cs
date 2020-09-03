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
    public class CLASEsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0601", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var cLASE = db.CLASE.Include(c => c.FAMILIA);
            return View(await cLASE.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASE cLASE = await db.CLASE.FindAsync(id);
            if (cLASE == null)
            {
                return HttpNotFound();
            }
            return View(cLASE);
        }

        // GET: CLASEs/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0602", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(cl => cl.nesta_fami== true), "ncode_fami", "sdesc_fami");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CLASE cLASE)
        {
            if (ModelState.IsValid)
            {
                db.CLASE.Add(cLASE);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(cl => cl.nesta_fami == true), "ncode_fami", "sdesc_fami");
            return View(cLASE);
        }

        // GET: CLASEs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0603", xcode);
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
            CLASE cLASE = await db.CLASE.FindAsync(id);
            if (cLASE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(cl => cl.nesta_fami == true), "ncode_fami", "sdesc_fami", cLASE.ncode_fami);
            return View(cLASE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CLASE cLASE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLASE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(cl => cl.nesta_fami == true), "ncode_fami", "sdesc_fami", cLASE.ncode_fami);
            return View(cLASE);
        }

        public async Task<ActionResult> DeleteClase(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0604", xcode);
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

            CLASE cLASE = await db.CLASE.FindAsync(id);
            db.CLASE.Remove(cLASE);
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
