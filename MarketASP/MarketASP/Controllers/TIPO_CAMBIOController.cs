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
    [Authorize]
    public class TIPO_CAMBIOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.TIPO_CAMBIO.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "dfecha_tc,ncompra_tc,nventa_tc")] TIPO_CAMBIO tIPO_CAMBIO)
        {
            if (ModelState.IsValid)
            {
                //verificar que exista tc del dia

                string xfecha = tIPO_CAMBIO.dfecha_tc.ToString();

                //string.Format("{0:d}", tIPO_CAMBIO.dfecha_tc)

                TIPO_CAMBIO _CAMBIO  = await db.TIPO_CAMBIO.FindAsync(DateTime.Parse(xfecha));
                if (_CAMBIO != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                db.TIPO_CAMBIO.Add(tIPO_CAMBIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }

            return View(tIPO_CAMBIO);
        }

        // GET: TIPO_CAMBIO/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            DateTime xid = DateTime.Parse(id);

            if (xid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPO_CAMBIO tIPO_CAMBIO = await db.TIPO_CAMBIO.FindAsync(xid);
            if (tIPO_CAMBIO == null)
            {
                return HttpNotFound();
            }
            return View(tIPO_CAMBIO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "dfecha_tc,ncompra_tc,nventa_tc")] TIPO_CAMBIO tIPO_CAMBIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIPO_CAMBIO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tIPO_CAMBIO);
        }

        public async Task<ActionResult> DeleteTC(string id)
        {
            DateTime xid = DateTime.Parse(id);

            if (xid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TIPO_CAMBIO tIPO_CAMBIO = await db.TIPO_CAMBIO.FindAsync(xid);
            db.TIPO_CAMBIO.Remove(tIPO_CAMBIO);
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
