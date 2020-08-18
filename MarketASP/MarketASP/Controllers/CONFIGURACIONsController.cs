using MarketASP.Models;
using MarketASP.Clases;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    public class CONFIGURACIONsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CONFIGURACIONs
        public async Task<ActionResult> Index()
        {
            return View(await db.CONFIGURACION.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_confi,sdesc_confi,svalor_confi,besta_confi,ntipo_confi")] CONFIGURACION cONFIGURACION)
        {
            if (ModelState.IsValid)
            {
                db.CONFIGURACION.Add(cONFIGURACION);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cONFIGURACION);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            if (cONFIGURACION == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_confi,sdesc_confi,svalor_confi,besta_confi,ntipo_confi")] CONFIGURACION cONFIGURACION)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cONFIGURACION).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ConfiguracionSingleton.confiCambio = true;
                return RedirectToAction("Index");
            }
            return View(cONFIGURACION);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            if (cONFIGURACION == null)
            {
                return HttpNotFound();
            }
            return View(cONFIGURACION);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CONFIGURACION cONFIGURACION = await db.CONFIGURACION.FindAsync(id);
            db.CONFIGURACION.Remove(cONFIGURACION);
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
