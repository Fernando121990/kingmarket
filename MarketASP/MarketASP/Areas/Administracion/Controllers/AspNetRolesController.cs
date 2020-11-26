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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using MarketASP.Models;

namespace MarketASP.Areas.Administracion.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AspNetRolesController : Controller
    {
        private static ApplicationDbContext userContext = new ApplicationDbContext();
        private MarketWebEntitiesAdmin db = new MarketWebEntitiesAdmin();

        // GET: Administracion/AspNetRoles
        public async Task<ActionResult> Index()
        {
            return View(await db.AspNetRoles.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(userContext));

                // Check to see if Role Exists, if not create it
                if (!roleManager.RoleExists(aspNetRoles.Name))
                {
                    roleManager.Create(new IdentityRole(aspNetRoles.Name));
                }
                return RedirectToAction("Index");
            }

            return View(aspNetRoles);
        }

        // GET: Administracion/AspNetRoles/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRoles aspNetRoles = await db.AspNetRoles.FindAsync(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRoles);
        }

        // POST: Administracion/AspNetRoles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetRoles).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(aspNetRoles);
        }


        public async Task<ActionResult> DeleteRol(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AspNetRoles aspNetRoles = await db.AspNetRoles.FindAsync(id);
            if (aspNetRoles == null)
            {
                return HttpNotFound();
            }

            db.AspNetRoles.Remove(aspNetRoles);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //public async Task<ActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AspNetRoles aspNetRoles = await db.AspNetRoles.FindAsync(id);
        //    if (aspNetRoles == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aspNetRoles);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(string id)
        //{
        //    AspNetRoles aspNetRoles = await db.AspNetRoles.FindAsync(id);
        //    db.AspNetRoles.Remove(aspNetRoles);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
