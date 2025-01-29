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
    public class UMEDIDAsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: UMEDIDAs
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0134", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(await db.UMEDIDA.ToListAsync());
        }

        // GET: UMEDIDAs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UMEDIDA uMEDIDA = await db.UMEDIDA.FindAsync(id);
            if (uMEDIDA == null)
            {
                return HttpNotFound();
            }
            return View(uMEDIDA);
        }

        // GET: UMEDIDAs/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0135", xcode);
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
        public async Task<ActionResult> Create([Bind(Include = "ncode_umed,sdesc_umed,ssunat_umed,scodsunat_umed,nesta_umed,suser_umed,dfech_umed,susmo_umed,dfemo_umed")] UMEDIDA uMEDIDA)
        {
            if (ModelState.IsValid)
            {
                db.UMEDIDA.Add(uMEDIDA);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(uMEDIDA);
        }

        // GET: UMEDIDAs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0136", xcode);
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
            UMEDIDA uMEDIDA = await db.UMEDIDA.FindAsync(id);
            if (uMEDIDA == null)
            {
                return HttpNotFound();
            }
            return View(uMEDIDA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_umed,sdesc_umed,ssunat_umed,scodsunat_umed,nesta_umed,suser_umed,dfech_umed,susmo_umed,dfemo_umed")] UMEDIDA uMEDIDA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uMEDIDA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(uMEDIDA);
        }

        public async Task<ActionResult> DeleteUmed(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0137", xcode);
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

            UMEDIDA uMEDIDA = await db.UMEDIDA.FindAsync(id);
            db.UMEDIDA.Remove(uMEDIDA);
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
