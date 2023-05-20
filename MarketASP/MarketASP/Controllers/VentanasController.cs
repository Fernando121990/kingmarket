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

namespace MarketASP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VentanasController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Administracion/Ventanas
        public async Task<ActionResult> Index()
        {
            return View(await db.Ventana.ToListAsync());
        }

        // GET: Administracion/Ventanas/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ventana ventana = await db.Ventana.FindAsync(id);
            if (ventana == null)
            {
                return HttpNotFound();
            }
            return View(ventana);
        }

        // GET: Administracion/Ventanas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Administracion/Ventanas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Nivel,Menu,Operacion")] Ventana ventana)
        {
            if (ModelState.IsValid)
            {
                db.Ventana.Add(ventana);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ventana);
        }

        // GET: Administracion/Ventanas/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ventana ventana = await db.Ventana.FindAsync(id);
            if (ventana == null)
            {
                return HttpNotFound();
            }
            return View(ventana);
        }

        // POST: Administracion/Ventanas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Nivel,Menu,Operacion")] Ventana ventana)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ventana).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ventana);
        }

        public async Task<ActionResult> DeleteNivel(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ventana ventana = await db.Ventana.FindAsync(id);
            if (ventana == null)
            {
                return HttpNotFound();
            }
            db.Ventana.Remove(ventana);
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
