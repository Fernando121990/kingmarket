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

        #region Articulos

        public async Task<ActionResult> Index()
        {
            var aRTICULO = db.ARTICULO.Include(a => a.FAMILIA).Include(a => a.CLASE).Include(a => a.MARCA).Include(a => a.UMEDIDA);
            return View(await aRTICULO.ToListAsync());
        }

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
        #endregion

        #region Precios
        public ActionResult CreatePrecio(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre");
            ViewBag.ncode_arti = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePrecio(ART_PRECIO aRT_PRECIO)
        {
            if (ModelState.IsValid)
            {
                aRT_PRECIO.suser_artpre = User.Identity.Name;
                aRT_PRECIO.nesta_artpre = true;
                aRT_PRECIO.dfech_artpre = DateTime.Today;

                db.ART_PRECIO.Add(aRT_PRECIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Articulos", new { id = aRT_PRECIO.ncode_arti });
            }

            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre", aRT_PRECIO.ncode_lipre);
            ViewBag.ncode_arti = aRT_PRECIO.ncode_arti;
            return View(aRT_PRECIO);
        }

        public async Task<ActionResult> DeletePrecio(int? id)
        {
            Int64 ncodeArti = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PRECIO aRT_PRECIO = await db.ART_PRECIO.FindAsync(id);
            if (aRT_PRECIO == null)
            {
                return HttpNotFound();
            }

            ncodeArti = aRT_PRECIO.ncode_arti;

            db.ART_PRECIO.Remove(aRT_PRECIO);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Articulos", new { id = ncodeArti });
        }

        public async Task<ActionResult> EditPrecio(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PRECIO aRT_PRECIO = await db.ART_PRECIO.FindAsync(id);
            if (aRT_PRECIO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre", aRT_PRECIO.ncode_lipre);
            ViewBag.ncode_arti = aRT_PRECIO.ncode_arti;
            return View(aRT_PRECIO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPrecio(ART_PRECIO aRT_PRECIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRT_PRECIO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Articulos", new { id = aRT_PRECIO.ncode_arti });
            }
            ViewBag.ncode_lipre = new SelectList(db.LISTA_PRECIO, "ncode_lipre", "sdesc_lipre", aRT_PRECIO.ncode_lipre);
            ViewBag.ncode_arti = aRT_PRECIO.ncode_arti;
            return View(aRT_PRECIO);
        }

        #endregion

        #region Barras

        public ActionResult CreateBarra(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ncode_arti = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateBarra(ART_BARRA aRT_BARRA)
        {
            if (ModelState.IsValid)
            {
                aRT_BARRA.nesta_barra = true;
                aRT_BARRA.suser_barra = User.Identity.Name;
                aRT_BARRA.dfech_barra = DateTime.Today;

                db.ART_BARRA.Add(aRT_BARRA);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Articulos", new { id = aRT_BARRA.ncode_arti });
            }

            ViewBag.ncode_arti = aRT_BARRA.ncode_arti;
            return View(aRT_BARRA);
        }

        public async Task<ActionResult> EditBarra(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_BARRA aRT_BARRA = await db.ART_BARRA.FindAsync(id);
            if (aRT_BARRA == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_arti = aRT_BARRA.ncode_arti;
            return View(aRT_BARRA);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditBarra(ART_BARRA aRT_BARRA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRT_BARRA).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Articulos", new { id = aRT_BARRA.ncode_arti });
            }
            ViewBag.ncode_arti = aRT_BARRA.ncode_arti;
            return View(aRT_BARRA);
        }


        public async Task<ActionResult> DeleteBarra(int? id)
        {
            Int64 ncodeArti = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_BARRA aRT_BARRA = await db.ART_BARRA.FindAsync(id);
            if (aRT_BARRA == null)
            {
                return HttpNotFound();
            }

            ncodeArti = aRT_BARRA.ncode_arti;
            db.ART_BARRA.Remove(aRT_BARRA);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Articulos", new { id = ncodeArti });
        }




        #endregion

        #region Proveedores
        public ActionResult CreateProveedor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ncode_arti = id;
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProveedor(ART_PROVE aRT_PROVE)
        {
            if (ModelState.IsValid)
            {
                aRT_PROVE.suser_arprove = User.Identity.Name;
                aRT_PROVE.dfech_arprove = DateTime.Today;

                db.ART_PROVE.Add(aRT_PROVE);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Articulos", new { id = aRT_PROVE.ncode_arti });
            }

            ViewBag.ncode_arti = aRT_PROVE.ncode_arti;
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", aRT_PROVE.ncode_provee);
            return View(aRT_PROVE);
        }

        public async Task<ActionResult> EditProveedor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PROVE aRT_PROVE = await db.ART_PROVE.FindAsync(id);
            if (aRT_PROVE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", aRT_PROVE.ncode_provee);
            return View(aRT_PROVE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProveedor(ART_PROVE aRT_PROVE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRT_PROVE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Articulos", new { id = aRT_PROVE.ncode_arti });
            }
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", aRT_PROVE.ncode_provee);
            return View(aRT_PROVE);
        }


        public async Task<ActionResult> DeleteProveedor(int? id)
        {
            Int64 ncodeArti = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ART_PROVE aRT_PROVE = await db.ART_PROVE.FindAsync(id);
            if (aRT_PROVE == null)
            {
                return HttpNotFound();
            }

            ncodeArti = aRT_PROVE.ncode_arti;
            db.ART_PROVE.Remove(aRT_PROVE);
            await db.SaveChangesAsync();
            return RedirectToAction("Details", "Articulos", new { id = ncodeArti });
        }


        #endregion
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
