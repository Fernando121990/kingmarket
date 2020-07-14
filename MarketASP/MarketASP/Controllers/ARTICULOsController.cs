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
    public class ARTICULOsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ARTICULOs
        public async Task<ActionResult> Index()
        {
            var aRTICULO = db.ARTICULO.Include(a => a.FAMILIA).Include(a => a.CLASE).Include(a => a.MARCA).Include(a => a.UMEDIDA);
            return View(await aRTICULO.ToListAsync());
        }

        // GET: ARTICULOs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICULO aRTICULO = await db.ARTICULO.FindAsync(id);
            if (aRTICULO == null)
            {
                return HttpNotFound();
            }
            return View(aRTICULO);
        }

        // GET: ARTICULOs/Create
        public ActionResult Create()
        {
            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(F => F.nesta_fami== true), "ncode_fami", "sdesc_fami");
            ViewBag.ncode_clase = new SelectList(db.CLASE.Where(C => C.nesta_clase == true), "ncode_clase", "sdesc_clase");
            ViewBag.ncode_marca = new SelectList(db.MARCA.Where(F => F.nesta_marca == true), "ncode_marca", "sdesc_marca");
            ViewBag.ncode_umed = new SelectList(db.UMEDIDA.Where(F => F.nesta_umed == true), "ncode_umed", "sdesc_umed");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ARTICULO aRTICULO)
        {
            if (ModelState.IsValid)
            {
                aRTICULO.suser_arti = User.Identity.Name;
                aRTICULO.dfech_arti = DateTime.Now;
                db.ARTICULO.Add(aRTICULO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(F => F.nesta_fami == true), "ncode_fami", "sdesc_fami", aRTICULO.ncode_fami);
            ViewBag.ncode_clase = new SelectList(db.CLASE.Where(C => C.nesta_clase == true), "ncode_clase", "sdesc_clase", aRTICULO.ncode_clase);
            ViewBag.ncode_marca = new SelectList(db.MARCA.Where(F => F.nesta_marca == true), "ncode_marca", "sdesc_marca", aRTICULO.ncode_marca);
            ViewBag.ncode_umed = new SelectList(db.UMEDIDA.Where(F => F.nesta_umed == true), "ncode_umed", "sdesc_umed", aRTICULO.ncode_umed);
            return View(aRTICULO);
        }

        // GET: ARTICULOs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICULO aRTICULO = await db.ARTICULO.FindAsync(id);
            if (aRTICULO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA.Where(F => F.nesta_fami == true), "ncode_fami", "sdesc_fami", aRTICULO.ncode_fami);
            ViewBag.ncode_clase = new SelectList(db.CLASE.Where(C => C.nesta_clase == true), "ncode_clase", "sdesc_clase", aRTICULO.ncode_clase);
            ViewBag.ncode_marca = new SelectList(db.MARCA.Where(F => F.nesta_marca == true), "ncode_marca", "sdesc_marca", aRTICULO.ncode_marca);
            ViewBag.ncode_umed = new SelectList(db.UMEDIDA.Where(F => F.nesta_umed == true), "ncode_umed", "sdesc_umed", aRTICULO.ncode_umed);
            return View(aRTICULO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_arti,sdesc1_arti,sdesc2_arti,sdescweb_arti,scode_arti,ncode_fami,ncode_clase,ncode_marca,ncode_umed,bisc_arti,bafecto_arti,bivap_arti,bpercepcion_arti,nporpercepcion_arti,bvenc_arti,sserie_arti,sobse_arti,nesta_arti,nstockmin_arti,nstockmax_arti,sabrev_arti,bdscto_arti,spaisOrigen_arti,bprocedencia_arti,suser_arti,dfech_arti,susmo_arti,dfemo_arti")] ARTICULO aRTICULO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRTICULO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fami = new SelectList(db.FAMILIA, "ncode_fami", "sdesc_fami", aRTICULO.ncode_fami);
            ViewBag.ncode_clase = new SelectList(db.CLASE, "ncode_clase", "sdesc_clase", aRTICULO.ncode_clase);
            ViewBag.ncode_marca = new SelectList(db.MARCA, "ncode_marca", "sdesc_marca", aRTICULO.ncode_marca);
            ViewBag.ncode_umed = new SelectList(db.UMEDIDA, "ncode_umed", "sdesc_umed", aRTICULO.ncode_umed);
            return View(aRTICULO);
        }

        public async Task<ActionResult> DeleteArti(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ARTICULO aRTICULO = await db.ARTICULO.FindAsync(id);
            db.ARTICULO.Remove(aRTICULO);
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
