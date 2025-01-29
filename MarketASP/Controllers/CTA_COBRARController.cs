using MarketASP.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using MarketASP.Helpers ;
using MarketASP.Clases;

namespace MarketASP.Controllers
{
    [Authorize]
    public class CTA_COBRARController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();
        private Funciones fnfunciones = new Funciones();

        // GET: CTA_COBRAR
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0601", xcode);
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

        public async Task<ActionResult> CreateCobro(long? id, string xtipo, string mensaje)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0602", xcode);
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
            

            ViewBag.ncode_ctaco = cTA_COBRAR.ncode_ctaco;
            ViewBag.ntotal = fnfunciones.FnRedondear(cTA_COBRAR.ntotal_ctaco, ConfiguracionSingleton.Instance.glbDecimales);
            ViewBag.tc = fnfunciones.FnRedondear(result.nventa_tc, ConfiguracionSingleton.Instance.glbDecimales);

            if (cTA_COBRAR.smone_ctaco == "MN")
            {
                ViewBag.nsaldoMN = fnfunciones.FnRedondear(cTA_COBRAR.ntotal_ctaco - cTA_COBRAR.npago_ctaco, ConfiguracionSingleton.Instance.glbDecimales);
                ViewBag.nsaldoUS = fnfunciones.FnRedondear( ViewBag.nsaldoMN / ViewBag.tc,ConfiguracionSingleton.Instance.glbDecimales);
            }
            else
            {

                ViewBag.nsaldoUS = fnfunciones.FnRedondear(cTA_COBRAR.ntotal_ctaco - cTA_COBRAR.npago_ctaco,ConfiguracionSingleton.Instance.glbDecimales);
                ViewBag.nsaldoMN = fnfunciones.FnRedondear(ViewBag.nsaldoUS * ViewBag.tc,ConfiguracionSingleton.Instance.glbDecimales);
            }

            ViewBag.sdocu_ctaco = cTA_COBRAR.sdocu_ctaco;
            ViewBag.smone_ctacode = cTA_COBRAR.smone_ctaco;
            ViewBag.ncode_banco = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 7), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_tpago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 8), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_tarjeta = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 9), "ncode_confi", "sdesc_confi");
            ViewBag.xtipo = xtipo;
            ViewBag.mensaje = mensaje;
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

            db.Pr_PermisoAcceso(User.Identity.Name, "0604", xcode);
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
