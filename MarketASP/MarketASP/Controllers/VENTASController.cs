using MarketASP.Clases;
using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace MarketASP.Controllers
{
    [Authorize]
    public class VENTASController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: VENTAS
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1301", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var vENTAS = db.VENTAS.Include(v => v.CLI_DIRE).Include(v => v.CLIENTE).Include(v => v.CONFIGURACION).Include(v => v.CONFIGURACION1);
            return View(await vENTAS.ToListAsync());
        }

        // GET: VENTAS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }
            return View(vENTAS);
        }

        // GET: VENTAS/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1302", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            

            ViewBag.smone_movi = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.smone_venta = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter cc = new ObjectParameter("cc", typeof(int));

            int code = 0;
            int cccode = 0;
            string data = "";
            int fila = 0;
            try
            {

                if (ModelState.IsValid)
                {
                    data = model_json;
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    if (data != null)
                    {
                        var mofView = JsonConvert.DeserializeObject<ventaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_ventaCrea(mofView.ncode_docu, mofView.sseri_venta, mofView.snume_venta, DateTime.Parse(mofView.sfeventa_venta),
                                DateTime.Parse(mofView.sfevenci_venta), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_venta, mofView.ntc_venta, mofView.ncode_fopago,
                                mofView.sobse_venta, mofView.ncode_compra, mofView.ncode_profo, mofView.nbrutoex_venta, mofView.nbrutoaf_venta,
                                mofView.ndctoex_venta, mofView.ndsctoaf_venta, mofView.nsubex_venta, mofView.nsubaf_venta, mofView.nigvex_venta,
                                mofView.nigvaf_venta, mofView.ntotaex_venta, mofView.ntotaaf_venta, mofView.ntotal_venta, mofView.ntotalMN_venta,
                                mofView.ntotalUs_venta, true, mofView.nvalIGV_venta, User.Identity.Name,mofView.ncode_alma,ConfiguracionSingleton.Instance.glbcobroAutomatico, sw,cc);


                            code = int.Parse(sw.Value.ToString());
                            cccode = int.Parse(cc.Value.ToString());

                            if (mofView.ventaViewDetas != null)
                            {
                                foreach (ventaViewDeta item in mofView.ventaViewDetas)
                                {
                                    fila++;
                                    db.Pr_ventaDetaCrea(code, item.ncode_arti, item.ncant_vedeta, item.npu_vedeta,
                                        item.ndscto_vedeta, item.ndscto2_vedeta, item.nexon_vedeta, item.nafecto_vedeta, item.besafecto_vedeta,
                                        item.ncode_alma, item.ndsctomax_vedeta, item.ndsctomin_vedeta, item.ndsctoporc_vedeta);
                                };

                            }

                            db.Pr_KardexCrea("Venta", 5, "S", code, User.Identity.Name);
                        }
                    }
                }
                if (ConfiguracionSingleton.Instance.glbcobroAutomatico == "SI")
                {
                    return Json(new { Success = 2, CtaCo = cccode });
                }

                return Json(new { Success = 1 });

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                ViewBag.mensaje = mensaje;
                return Json(new { Success = 0 });
            }
        }


        // GET: VENTAS/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1303", xcode);
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
            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }
            ViewBag.smone_venta = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi",vENTAS.smone_venta);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi",vENTAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi",vENTAS.ncode_fopago);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma",vENTAS.ncode_alma);
            ViewBag.sdesc_cliente = vENTAS.CLIENTE.srazon_cliente;
            ViewBag.NRO_DCLIENTE = new SelectList(db.CLI_DIRE.Where(c => c.ncode_cliente == vENTAS.ncode_cliente), "ncode_clidire", "sdesc_clidire", vENTAS.ncode_clidire);
            return View(vENTAS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

            int xsw;
            long code;
            string data = "";
            int fila = 0;
            try
            {

                if (ModelState.IsValid)
                {
                    data = model_json;
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    if (data != null)
                    {
                        var mofView = JsonConvert.DeserializeObject<ventaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_ventaEdita(mofView.ncode_venta, mofView.ncode_docu, mofView.dfeventa_venta,
                                mofView.dfevenci_venta, mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_venta, mofView.ntc_venta, mofView.ncode_fopago,
                                mofView.sobse_venta, mofView.ncode_compra, mofView.ncode_profo, mofView.nbrutoex_venta, mofView.nbrutoaf_venta,
                                mofView.ndctoex_venta, mofView.ndsctoaf_venta, mofView.nsubex_venta, mofView.nsubaf_venta, mofView.nigvex_venta,
                                mofView.nigvaf_venta, mofView.ntotaex_venta, mofView.ntotaaf_venta, mofView.ntotal_venta, mofView.ntotalMN_venta,
                                mofView.ntotalUs_venta, mofView.nvalIGV_venta, User.Identity.Name, mofView.ncode_alma, sw);


                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_venta;

                            if (mofView.ventaViewDetas != null)
                            {
                                foreach (ventaViewDeta item in mofView.ventaViewDetas)
                                {
                                    fila++;
                                    db.Pr_ventaDetaCrea(code, item.ncode_arti, item.ncant_vedeta, item.npu_vedeta,
                                        item.ndscto_vedeta, item.ndscto2_vedeta, item.nexon_vedeta, item.nafecto_vedeta, item.besafecto_vedeta,
                                        item.ncode_alma, item.ndsctomax_vedeta, item.ndsctomin_vedeta, item.ndsctoporc_vedeta);
                                };

                            }

                            db.Pr_KardexCrea("Venta", 5, "S", code, User.Identity.Name);

                            db.Pr_ventaDetaEdita(code);
                        }
                    }
                }

                return Json(new { Success = 1 });

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                ViewBag.mensaje = mensaje;
                return Json(new { Success = 0 });
            }
        }


        public async Task<ActionResult> anulaVenta(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1305", xcode);
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

            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_ventaAnula(id, sw);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteVenta(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1304", xcode);
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

            VENTAS vENTAS = await db.VENTAS.FindAsync(id);
            if (vENTAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_ventaElimina(id,sw);
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
