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
    public class UMEDIDAsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: UMEDIDAs
        public async Task<ActionResult> Index()
        {
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

        // POST: UMEDIDAs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
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
