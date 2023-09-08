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
    public class FabricacionsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Fabricacions
        public async Task<ActionResult> Index()
        {
            return View(await db.Fabricacion.ToListAsync());
        }

        // GET: Fabricacions/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            if (fabricacion == null)
            {
                return HttpNotFound();
            }
            return View(fabricacion);
        }

        // GET: Fabricacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fabricacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Fab_Tipo,Fab_NroDoc,Fab_Fecha,Fab_TipoCambio,Fab_Glosa,Fab_Lote,Fab_Estado,Fab_TipoProd,Fab_Fvenc,Fab_Cantidad,Fab_Codigo,Rec_Codigo,Fab_CostoUnit,Fab_CostoTotalMN,Fab_CostoTotalUS,Fab_CostoOperativo,Fab_almacen")] Fabricacion fabricacion)
        {
            if (ModelState.IsValid)
            {
                db.Fabricacion.Add(fabricacion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fabricacion);
        }

        // GET: Fabricacions/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            if (fabricacion == null)
            {
                return HttpNotFound();
            }
            return View(fabricacion);
        }

        // POST: Fabricacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Fab_Tipo,Fab_NroDoc,Fab_Fecha,Fab_TipoCambio,Fab_Glosa,Fab_Lote,Fab_Estado,Fab_TipoProd,Fab_Fvenc,Fab_Cantidad,Fab_Codigo,Rec_Codigo,Fab_CostoUnit,Fab_CostoTotalMN,Fab_CostoTotalUS,Fab_CostoOperativo,Fab_almacen")] Fabricacion fabricacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fabricacion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fabricacion);
        }

        // GET: Fabricacions/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            if (fabricacion == null)
            {
                return HttpNotFound();
            }
            return View(fabricacion);
        }

        // POST: Fabricacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            db.Fabricacion.Remove(fabricacion);
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
