﻿using System;
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
    public class LOCALsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1022", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }
            var lOCAL = db.LOCAL.Include(l => l.SUCURSAL);
            return View(await lOCAL.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOCAL lOCAL = await db.LOCAL.FindAsync(id);
            if (lOCAL == null)
            {
                return HttpNotFound();
            }
            return View(lOCAL);
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1023", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }
            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s =>s.bacti_sucu==true).OrderByDescending(s=>s.sdesc_sucu), "ncode_sucu", "sdesc_sucu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_local,sdesc_local,sdire_local,scode_ubigeo,bacti_local,ncode_sucu")] LOCAL lOCAL)
        {
            if (ModelState.IsValid)
            {
                db.LOCAL.Add(lOCAL);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s => s.bacti_sucu == true).OrderByDescending(s => s.sdesc_sucu), "ncode_sucu", "sdesc_sucu", lOCAL.ncode_sucu);
            return View(lOCAL);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1024", xcode);
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
            LOCAL lOCAL = await db.LOCAL.FindAsync(id);
            if (lOCAL == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s => s.bacti_sucu == true).OrderByDescending(s => s.sdesc_sucu), "ncode_sucu", "sdesc_sucu", lOCAL.ncode_sucu);
            return View(lOCAL);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_local,sdesc_local,sdire_local,scode_ubigeo,bacti_local,ncode_sucu")] LOCAL lOCAL)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOCAL).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            
            ViewBag.ncode_sucu = new SelectList(db.SUCURSAL.Where(s => s.bacti_sucu == true).OrderByDescending(s => s.sdesc_sucu), "ncode_sucu", "sdesc_sucu", lOCAL.ncode_sucu);
            return View(lOCAL);
        }

        public async Task<ActionResult> DeleteLocal(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1025", xcode);
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

            LOCAL lOCAL  = await db.LOCAL.FindAsync(id);
            if (lOCAL == null)
            {
                return HttpNotFound();
            }
            db.LOCAL.Remove(lOCAL);
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
