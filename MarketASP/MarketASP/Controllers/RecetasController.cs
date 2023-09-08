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
    public class RecetasController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Recetas
        public async Task<ActionResult> Index()
        {
            return View(await db.Receta.ToListAsync());
        }

        // GET: Recetas/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receta receta = await db.Receta.FindAsync(id);
            if (receta == null)
            {
                return HttpNotFound();
            }
            return View(receta);
        }

        // GET: Recetas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recetas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Rec_codigo,Rec_descripcion,Rec_codclase,Rec_codProd,Rec_cantidad,Rec_almacen,Rec_tipo,Rec_costoOperativo")] Receta receta)
        {
            if (ModelState.IsValid)
            {
                db.Receta.Add(receta);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(receta);
        }

        // GET: Recetas/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receta receta = await db.Receta.FindAsync(id);
            if (receta == null)
            {
                return HttpNotFound();
            }
            return View(receta);
        }

        // POST: Recetas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Rec_codigo,Rec_descripcion,Rec_codclase,Rec_codProd,Rec_cantidad,Rec_almacen,Rec_tipo,Rec_costoOperativo")] Receta receta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receta).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(receta);
        }

        // GET: Recetas/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receta receta = await db.Receta.FindAsync(id);
            if (receta == null)
            {
                return HttpNotFound();
            }
            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Receta receta = await db.Receta.FindAsync(id);
            db.Receta.Remove(receta);
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
