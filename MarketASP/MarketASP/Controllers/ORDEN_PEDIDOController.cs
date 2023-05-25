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
using MarketASP.Clases;
using Newtonsoft.Json;
using MarketASP.Extensiones;

namespace MarketASP.Controllers
{
    public class ORDEN_PEDIDOController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ORDEN_PEDIDOS
        public async Task<ActionResult> Index()
        {
            var ORDEN_PEDIDOS = db.ORDEN_PEDIDOS.Include(p => p.ALMACEN).Include(p => p.CLI_DIRE).Include(p => p.CLIENTE).Include(p => p.CONFIGURACION).Include(p => p.CONFIGURACION1).Include(p => p.LOCAL);
            return View(await ORDEN_PEDIDOS.ToListAsync());
        }

        // GET: ORDEN_PEDIDOS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ORDEN_PEDIDOS oRDEN_PEDIDOS = await db.ORDEN_PEDIDOS.FindAsync(id);
            if (oRDEN_PEDIDOS == null)
            {
                return HttpNotFound();
            }
            return View(oRDEN_PEDIDOS);
        }

        // GET: ORDEN_PEDIDOS/Create
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
                ViewBag.mensaje = "No se ha registrado el tipo de cambio, comuniquese con el administrador del sistema";
                return View("_Mensaje");
                //                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5 && c.ncode_confi == 1066), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.dfeorpeo_orpe = string.Format("{0:d}", yfecha);
            ViewBag.dfevenci_orpe = string.Format("{0:d}", yfecha);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter cc = new ObjectParameter("cc", typeof(int));

            int code = 0;
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
                        var mofView = JsonConvert.DeserializeObject<ordenpedidoView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_Orden_PedidoCrea(mofView.ncode_docu, mofView.sseri_orpe, mofView.snume_orpe, DateTime.Parse(mofView.sfeordenpedido_orpe),
                                DateTime.Parse(mofView.sfevenci_orpe), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_orpe, mofView.ntc_orpe, mofView.ncode_fopago,
                                mofView.sobse_orpe, mofView.ncode_compra, mofView.nbrutoex_orpe, mofView.nbrutoaf_orpe,
                                mofView.ndctoex_orpe, mofView.ndsctoaf_orpe, mofView.nsubex_orpe, mofView.nsubaf_orpe, mofView.nigvex_orpe,
                                mofView.nigvaf_orpe, mofView.ntotaex_orpe, mofView.ntotaaf_orpe, mofView.ntotal_orpe, mofView.ntotalMN_orpe,
                                mofView.ntotalUs_orpe, true, mofView.nvalIGV_orpe, User.Identity.Name, mofView.ncode_alma, int.Parse(User.Identity.GetLocal()), mofView.ncode_mone ,
                                sw);


                            code = int.Parse(sw.Value.ToString());
                            //cccode = int.Parse(cc.Value.ToString());

                            if (mofView.ordenpedidoViewDetas != null)
                            {
                                foreach (ordenpedidoViewDeta item in mofView.ordenpedidoViewDetas)
                                {
                                    fila++;
                                    db.Pr_Orden_PedidoDetaCrea(code, item.ncode_arti, item.ncant_orpedeta, item.npu_orpedeta,
                                        item.ndscto_orpedeta, item.ndscto2_orpedeta, item.nexon_orpedeta, item.nafecto_orpedeta, item.besafecto_orpedeta,
                                        item.ncode_alma, item.ndsctomax_orpedeta, item.ndsctomin_orpedeta, item.ndsctoporc_orpedeta);
                                };

                            }

                            db.Pr_KardexElimina("pedido", code);

                            db.Pr_KardexCrea("PEDIDO", 1039, "R", code, User.Identity.Name);
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
            ORDEN_PEDIDOS oRDEN_PEDIDOS = await db.ORDEN_PEDIDOS.FindAsync(id);
            if (oRDEN_PEDIDOS == null)
            {
                return HttpNotFound();
            }
            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", oRDEN_PEDIDOS.smone_orpe);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5 && c.ncode_confi == 1066), "ncode_confi", "sdesc_confi", oRDEN_PEDIDOS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", oRDEN_PEDIDOS.ncode_fopago);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", oRDEN_PEDIDOS.ncode_alma);
            ViewBag.cod_cliente = oRDEN_PEDIDOS.ncode_cliente;
            ViewBag.sdesc_cliente = oRDEN_PEDIDOS.CLIENTE.srazon_cliente;
            ViewBag.sruc_cliente = oRDEN_PEDIDOS.CLIENTE.sruc_cliente;
            ViewBag.sdni_cliente = oRDEN_PEDIDOS.CLIENTE.sdnice_cliente;
            ViewBag.dfeorpeo_orpe = string.Format("{0:d}", oRDEN_PEDIDOS.dfeorpeo_orpe);
            ViewBag.dfevenci_orpe = string.Format("{0:d}", oRDEN_PEDIDOS.dfevenci_orpe);
            ViewBag.tc = oRDEN_PEDIDOS.ntc_orpe;
            ViewBag.NRO_DCLIENTE = new SelectList(db.CLI_DIRE.Where(c => c.ncode_cliente == oRDEN_PEDIDOS.ncode_cliente), "ncode_clidire", "sdesc_clidire", oRDEN_PEDIDOS.ncode_clidire);
            return View(oRDEN_PEDIDOS);
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
                        var mofView = JsonConvert.DeserializeObject<ordenpedidoView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_Orden_PedidoEdita(mofView.ncode_orpe, mofView.ncode_docu, DateTime.Parse(mofView.sfeordenpedido_orpe),
                                DateTime.Parse(mofView.sfevenci_orpe), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_orpe, mofView.ntc_orpe, mofView.ncode_fopago,
                                mofView.sobse_orpe, mofView.ncode_compra, mofView.nbrutoex_orpe, mofView.nbrutoaf_orpe,
                                mofView.ndctoex_orpe, mofView.ndsctoaf_orpe, mofView.nsubex_orpe, mofView.nsubaf_orpe, mofView.nigvex_orpe,
                                mofView.nigvaf_orpe, mofView.ntotaex_orpe, mofView.ntotaaf_orpe, mofView.ntotal_orpe, mofView.ntotalMN_orpe,
                                mofView.ntotalUs_orpe,true, mofView.nvalIGV_orpe, User.Identity.Name, mofView.ncode_alma, int.Parse(User.Identity.GetLocal()), mofView.ncode_mone, sw);


                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_orpe;

                            if (mofView.ordenpedidoViewDetas != null)
                            {
                                foreach (ordenpedidoViewDeta item in mofView.ordenpedidoViewDetas)
                                {
                                    fila++;
                                    db.Pr_Orden_PedidoDetaCrea(code, item.ncode_arti, item.ncant_orpedeta, item.npu_orpedeta,
                                        item.ndscto_orpedeta, item.ndscto2_orpedeta, item.nexon_orpedeta, item.nafecto_orpedeta, item.besafecto_orpedeta,
                                        item.ncode_alma, item.ndsctomax_orpedeta, item.ndsctomin_orpedeta, item.ndsctoporc_orpedeta);
                                };

                            }

                            db.Pr_Orden_PedidoDetaEdita(code);

                            db.Pr_KardexElimina("PEDIDO", code);
                            db.Pr_KardexCrea("PEDIDO", 1039, "R", code, User.Identity.Name);

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

        public async Task<ActionResult> DeleteOrden_Pedido(int? id)
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

            ORDEN_PEDIDOS oRDEN_PEDIDOS = await db.ORDEN_PEDIDOS.FindAsync(id);
            if (oRDEN_PEDIDOS == null)
            {
                return HttpNotFound();
            }

            db.Pr_OrdenPedidoElimina(id, sw);
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
