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
    public class CTA_COBRARController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CTA_COBRAR
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1501", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var cTA_COBRAR = db.CTA_COBRAR.Include(c => c.CLIENTE).Include(c => c.CONFIGURACION);
            return View(await cTA_COBRAR.ToListAsync());
        }

        // GET: CTA_COBRAR/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
            if (cTA_COBRAR == null)
            {
                return HttpNotFound();
            }
            return View(cTA_COBRAR);
        }

        public async Task<ActionResult> CreateCobro(long? id, string xtipo)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1502", xcode);
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
            CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
            if (cTA_COBRAR == null)
            {
                return HttpNotFound();
            }
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;

            ViewBag.ncode_ctaco = cTA_COBRAR.ncode_ctaco;
            ViewBag.ntotal = cTA_COBRAR.ntotal_ctaco;

            if (cTA_COBRAR.smone_ctaco == "MN")
            {
                ViewBag.nsaldoMN = cTA_COBRAR.ntotal_ctaco - cTA_COBRAR.npago_ctaco;
                ViewBag.nsaldoUS = ViewBag.nsaldoMN / ViewBag.tc;
            }
            else
            {

                ViewBag.nsaldoUS = cTA_COBRAR.ntotal_ctaco - cTA_COBRAR.npago_ctaco;
                ViewBag.nsaldoMN = ViewBag.nsaldoUS * ViewBag.tc;
            }

            ViewBag.smone_ctacode = cTA_COBRAR.smone_ctaco;
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 7), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 8), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_tarjeta = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 9), "ncode_confi", "sdesc_confi");
            ViewBag.xtipo = xtipo;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCobro(CTASCO_DETALLE cTASCO_DETALLE,string xtipo)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            if (ModelState.IsValid)
            {
                db.Pr_ctaCobrarDetaCrea(cTASCO_DETALLE.dfepago_ctacode, cTASCO_DETALLE.sdoc_ctacode, cTASCO_DETALLE.smone_ctacode, cTASCO_DETALLE.ntc_ctacode,
                    cTASCO_DETALLE.nmonto_ctacode, cTASCO_DETALLE.nmontoMN_ctacode, cTASCO_DETALLE.nmontoUS_ctacode, cTASCO_DETALLE.nvuelto_ctacode,
                    cTASCO_DETALLE.ncode_tarjeta, cTASCO_DETALLE.suser_tarjeta, cTASCO_DETALLE.snro_tarjeta, cTASCO_DETALLE.sobse_ctacode, User.Identity.Name,
                    cTASCO_DETALLE.ncode_ctaco, cTASCO_DETALLE.ncode_banco, cTASCO_DETALLE.ncode_tpago, sw);

                switch (xtipo.ToUpper())
                {
                    case "VENTA":
                        return RedirectToAction("Create","Ventas");
                    case "CTACO":
                        return RedirectToAction("Index");
                    default:
                        break;
                }
                
            }
            ViewBag.mensaje = "No se puede registrar Pago";
            return View("Error");
        }

        public async Task<ActionResult> DeleteCobro(int? id)
        {

            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1503", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CTASCO_DETALLE cTASCO_DETALLE = await db.CTASCO_DETALLE.FindAsync(id);
            if (cTASCO_DETALLE == null)
            {
                return HttpNotFound();
            }

            db.Pr_ctaCobrarDetaElimina(cTASCO_DETALLE.ncode_ctacode, cTASCO_DETALLE.ncode_ctaco, cTASCO_DETALLE.CTA_COBRAR.ncodeDoc_ctaco, User.Identity.Name, sw);
            return RedirectToAction("Index");
        }


        //// GET: CTA_COBRAR/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente");
        //    ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ncode_ctaco,ncodeDoc_ctaco,sdocu_ctaco,dfecta_ctaco,smone_ctaco,ntotal_ctaco,dfevenci_ctaco,ntc_ctaco,ntotalMN_ctaco,ntotalUS_ctaco,npago_ctaco,ncode_letra,sesta_letra,suser_ctaco,dfech_ctaco,susmo_ctaco,dfemo_ctaco,ncode_cliente,ncode_docu")] CTA_COBRAR cTA_COBRAR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CTA_COBRAR.Add(cTA_COBRAR);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cTA_COBRAR.ncode_cliente);
        //    ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTA_COBRAR.ncode_docu);
        //    return View(cTA_COBRAR);
        //}

        // GET: CTA_COBRAR/Edit/5
        //public async Task<ActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
        //    if (cTA_COBRAR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cTA_COBRAR.ncode_cliente);
        //    ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTA_COBRAR.ncode_docu);
        //    return View(cTA_COBRAR);
        //}

        //// POST: CTA_COBRAR/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ncode_ctaco,ncodeDoc_ctaco,sdocu_ctaco,dfecta_ctaco,smone_ctaco,ntotal_ctaco,dfevenci_ctaco,ntc_ctaco,ntotalMN_ctaco,ntotalUS_ctaco,npago_ctaco,ncode_letra,sesta_letra,suser_ctaco,dfech_ctaco,susmo_ctaco,dfemo_ctaco,ncode_cliente,ncode_docu")] CTA_COBRAR cTA_COBRAR)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cTA_COBRAR).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cTA_COBRAR.ncode_cliente);
        //    ViewBag.ncode_docu = new SelectList(db.CONFIGURACION, "ncode_confi", "sdesc_confi", cTA_COBRAR.ncode_docu);
        //    return View(cTA_COBRAR);
        //}

        // GET: CTA_COBRAR/Delete/5
        //public async Task<ActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
        //    if (cTA_COBRAR == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cTA_COBRAR);
        //}

        //// POST: CTA_COBRAR/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(long id)
        //{
        //    CTA_COBRAR cTA_COBRAR = await db.CTA_COBRAR.FindAsync(id);
        //    db.CTA_COBRAR.Remove(cTA_COBRAR);
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
