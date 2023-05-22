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
    public class PROVEEDORsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1101", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(await db.PROVEEDOR.ToListAsync());
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1102", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PROVEEDOR pROVEEDOR)
        {
            if (ModelState.IsValid)
            {
                db.PROVEEDOR.Add(pROVEEDOR);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            return View(pROVEEDOR);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1103", xcode);
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
            PROVEEDOR pROVEEDOR = await db.PROVEEDOR.FindAsync(id);
            if (pROVEEDOR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi",pROVEEDOR.ncode_fopago);
            return View(pROVEEDOR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_provee,sdesc_prove,sdire_prove,sruc_prove,sfono_prove,sfax_prove,smail_prove,sobse_prove,sweb_prove,scontac_prove,nesta_prove,scargoconta_prove,bprocedencia_prove,suser_prove,dfech_prove,susmo_prove,dfemo_prove")] PROVEEDOR pROVEEDOR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pROVEEDOR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", pROVEEDOR.ncode_fopago);
            return View(pROVEEDOR);
        }

        public async Task<ActionResult> DeleteProve(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1104", xcode);
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
            PROVEEDOR pROVEEDOR = await db.PROVEEDOR.FindAsync(id);
            if (pROVEEDOR == null)
            {
                return HttpNotFound();
            }
            db.PROVEEDOR.Remove(pROVEEDOR);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        //// POST: PROVEEDORs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    PROVEEDOR pROVEEDOR = await db.PROVEEDOR.FindAsync(id);
        //    db.PROVEEDOR.Remove(pROVEEDOR);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
