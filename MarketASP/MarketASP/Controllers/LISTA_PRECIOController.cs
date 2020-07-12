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
    public class LISTA_PRECIOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: LISTA_PRECIO
        public async Task<ActionResult> Index()
        {
            return View(await db.LISTA_PRECIO.ToListAsync());
        }

        // GET: LISTA_PRECIO/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LISTA_PRECIO lISTA_PRECIO = await db.LISTA_PRECIO.FindAsync(id);
            if (lISTA_PRECIO == null)
            {
                return HttpNotFound();
            }
            return View(lISTA_PRECIO);
        }

        // GET: LISTA_PRECIO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LISTA_PRECIO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_lipre,sdesc_lipre,nesta_lipre,suser_lipre,dfech_lipre,susmo_lipre,dfemo_lipre")] LISTA_PRECIO lISTA_PRECIO)
        {
            if (ModelState.IsValid)
            {
                db.LISTA_PRECIO.Add(lISTA_PRECIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lISTA_PRECIO);
        }

        // GET: LISTA_PRECIO/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LISTA_PRECIO lISTA_PRECIO = await db.LISTA_PRECIO.FindAsync(id);
            if (lISTA_PRECIO == null)
            {
                return HttpNotFound();
            }
            return View(lISTA_PRECIO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_lipre,sdesc_lipre,nesta_lipre,suser_lipre,dfech_lipre,susmo_lipre,dfemo_lipre")] LISTA_PRECIO lISTA_PRECIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lISTA_PRECIO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lISTA_PRECIO);
        }

        public async Task<ActionResult> DeleteLPrecio(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LISTA_PRECIO lISTA_PRECIO = await db.LISTA_PRECIO.FindAsync(id);
            db.LISTA_PRECIO.Remove(lISTA_PRECIO);
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
