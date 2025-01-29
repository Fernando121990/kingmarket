using MarketASP.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    [Authorize]
    public class KARDEXesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index(string fini,string ffin, int salmacen = 0, int sarticulo = 0, int slinea = 0, 
            int chkalmacen = 0, int chkarticulo = 0, int chktipo = 0, int chklinea = 0)
        {
            int xvalue = 0; //DateTime xfini; DateTime xffin; 
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0205", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            if (string.IsNullOrEmpty(fini))
            {
                fini = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
                //xfini = DateTime.Today.AddDays(-1);

            }

            if (string.IsNullOrEmpty(ffin))
            {

                ffin = DateTime.Today.ToString("dd/MM/yyyy");
                //xffin = DateTime.Today;
            }

            if (chklinea == 1)
            {
                slinea = 0;
                ViewBag.chklinea = "Checked";
            }

            if (chkalmacen == 1)
            {
                salmacen = 0;
                ViewBag.chkalmacen = "Checked";
            }

            if (chkarticulo == 1)
            {
                sarticulo = 0;
                ViewBag.chkarticulo = "Checked";
            }

            if (chktipo == 1)
            {
                ViewBag.chktipo = "Checked";
            }


            ViewBag.fini = fini;
            ViewBag.ffin = ffin;
            ViewBag.salmacen = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.sarticulo = new SelectList(db.ARTICULO.Where(c => c.nesta_arti == true), "ncode_arti", "sdesc1_arti");
            ViewBag.slinea = new SelectList(db.LINEA.Where(c => c.nesta_linea == true), "ncode_linea", "sdesc_linea");


            var resultado = db.Pr_KardexMovimientosArticulo(fini,ffin,"", sarticulo,salmacen,slinea).ToList();

            return View(resultado);
        }
        public async Task<ActionResult> Resumen(int salmacen = 0, int sarticulo = 0, int slinea = 0,
            int chkalmacen = 0, int chkarticulo = 0, int chktipo = 0, int chklinea = 0)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0206", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            //if (string.IsNullOrEmpty(fini))
            //{
            //    fini = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy");
            //    //xfini = DateTime.Today.AddDays(-1);

            //}

            //if (string.IsNullOrEmpty(ffin))
            //{

            //    ffin = DateTime.Today.ToString("dd/MM/yyyy");
            //    //xffin = DateTime.Today;
            //}

            if (chklinea == 1)
            {
                slinea = 0;
                ViewBag.chklinea = "Checked";
            }

            if (chkalmacen == 1)
            {
                salmacen = 0;
                ViewBag.chkalmacen = "Checked";
            }

            if (chkarticulo == 1)
            {
                sarticulo = 0;
                ViewBag.chkarticulo = "Checked";
            }

            if (chktipo == 1)
            {
                ViewBag.chktipo = "Checked";
            }


            //ViewBag.fini = fini;
            //ViewBag.ffin = ffin;
            ViewBag.salmacen = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.sarticulo = new SelectList(db.ARTICULO.Where(c => c.nesta_arti == true), "ncode_arti", "sdesc1_arti");
            ViewBag.slinea = new SelectList(db.LINEA.Where(c => c.nesta_linea == true), "ncode_linea", "sdesc_linea");

            var kARDEX = db.Pr_KardexArticulos(sarticulo,"", "",salmacen,slinea).ToList();
            return View(kARDEX);
        }
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            return View(kARDEX);
        }

        public ActionResult Create()
        {
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_kardex,dfekard_kardex,stipomovi_kardex,ncant_kardex,npuco_kardex,ntc_kardex,smone_kardex,npucoMN_kardex,npucoUS_kardex,ncodeDoc_kardex,sserie_kardex,snume_kardex,dfvence_kardex,suser_kardex,dfech_kardex,susmo_kardex,dfemo_kardex,ncode_alma")] KARDEX kARDEX)
        {
            if (ModelState.IsValid)
            {
                db.KARDEX.Add(kARDEX);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_kardex,dfekard_kardex,stipomovi_kardex,ncant_kardex,npuco_kardex,ntc_kardex,smone_kardex,npucoMN_kardex,npucoUS_kardex,ncodeDoc_kardex,sserie_kardex,snume_kardex,dfvence_kardex,suser_kardex,dfech_kardex,susmo_kardex,dfemo_kardex,ncode_alma")] KARDEX kARDEX)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kARDEX).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            return View(kARDEX);
        }

        // POST: KARDEXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            db.KARDEX.Remove(kARDEX);
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
