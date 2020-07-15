using MarketASP.Models;
using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    [Authorize]
    public class FAMILIAsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.FAMILIA.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FAMILIA fAMILIA)
        {
            if (ModelState.IsValid)
            {
                fAMILIA.nesta_fami = true;
                fAMILIA.suser_fami = User.Identity.Name;
                fAMILIA.dfech_fami = DateTime.Now;
                db.FAMILIA.Add(fAMILIA);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(fAMILIA);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FAMILIA fAMILIA = await db.FAMILIA.FindAsync(id);
            if (fAMILIA == null)
            {
                return HttpNotFound();
            }
            return View(fAMILIA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FAMILIA fAMILIA)
        {
            if (ModelState.IsValid)
            {
                fAMILIA.susmo_fami = User.Identity.Name;
                fAMILIA.dfemo_fami = DateTime.Now;

                db.Entry(fAMILIA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(fAMILIA);
        }

        public async Task<ActionResult> DeleteFami(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            FAMILIA fAMILIA = await db.FAMILIA.FindAsync(id);
            db.FAMILIA.Remove(fAMILIA);
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
