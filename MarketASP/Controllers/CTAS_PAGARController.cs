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
using System.Data.Entity.Core.Objects;

namespace MarketASP.Controllers
{
    [Authorize]
    public class CTAS_PAGARController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CTAS_PAGAR
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0605", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var cTAS_PAGAR = db.CTAS_PAGAR.Include(c => c.CONFIGURACION).Include(c => c.CONFIGURACION1).Include(c => c.PROVEEDOR);
            return View(await cTAS_PAGAR.ToListAsync());
        }

        // GET: CTAS_PAGAR/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTAS_PAGAR cTAS_PAGAR = await db.CTAS_PAGAR.FindAsync(id);
            if (cTAS_PAGAR == null)
            {
                return HttpNotFound();
            }
            return View(cTAS_PAGAR);
        }

        // GET: CTAS_PAGAR/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0606", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_ctapa,ncodeDoc_ctapa,sdocu_ctapa,dfecta_ctapa,dfevenci_ctapa,smone_ctapa,ntotal_ctapa,ntotalMN_ctapa,ntotal_US_ctapa,ntc_ctapa,npago_ctapa,ncode_letra,sesta_letra,suser_ctapa,dfech_ctapa,susmo_ctapa,dfemo_ctapa,ncode_docu,ncode_provee,ncode_banco")] CTAS_PAGAR cTAS_PAGAR)
        {
            if (ModelState.IsValid)
            {
                db.CTAS_PAGAR.Add(cTAS_PAGAR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTAS_PAGAR.ncode_docu);
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTAS_PAGAR.ncode_banco);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", cTAS_PAGAR.ncode_provee);
            return View(cTAS_PAGAR);
        }

        // GET: CTAS_PAGAR/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {

            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0607", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTAS_PAGAR cTAS_PAGAR = await db.CTAS_PAGAR.FindAsync(id);
            if (cTAS_PAGAR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTAS_PAGAR.ncode_docu);
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTAS_PAGAR.ncode_banco);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", cTAS_PAGAR.ncode_provee);
            return View(cTAS_PAGAR);
        }

        // POST: CTAS_PAGAR/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_ctapa,ncodeDoc_ctapa,sdocu_ctapa,dfecta_ctapa,dfevenci_ctapa,smone_ctapa,ntotal_ctapa,ntotalMN_ctapa,ntotal_US_ctapa,ntc_ctapa,npago_ctapa,ncode_letra,sesta_letra,suser_ctapa,dfech_ctapa,susmo_ctapa,dfemo_ctapa,ncode_docu,ncode_provee,ncode_banco")] CTAS_PAGAR cTAS_PAGAR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTAS_PAGAR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTAS_PAGAR.ncode_docu);
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTAS_PAGAR.ncode_banco);
            ViewBag.ncode_provee = new SelectList(db.PROVEEDOR, "ncode_provee", "sdesc_prove", cTAS_PAGAR.ncode_provee);
            return View(cTAS_PAGAR);
        }

        // GET: CTAS_PAGAR/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0608", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTAS_PAGAR cTAS_PAGAR = await db.CTAS_PAGAR.FindAsync(id);
            if (cTAS_PAGAR == null)
            {
                return HttpNotFound();
            }
            return View(cTAS_PAGAR);
        }

        // POST: CTAS_PAGAR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            CTAS_PAGAR cTAS_PAGAR = await db.CTAS_PAGAR.FindAsync(id);
            db.CTAS_PAGAR.Remove(cTAS_PAGAR);
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
