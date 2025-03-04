﻿using System;
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
using static MarketASP.Clases.Enum;
using System.IO;

using Rotativa;

namespace MarketASP.Controllers
{
    [Authorize]
    public class ORDEN_PEDIDOController : BaseController
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: ORDEN_PEDIDOS
        public ActionResult Index(string fini,string ffin, string cliente, string vendedor, string documento,
            int chkpendiente = 0, int chkparcial = 0, int chktotal = 0)
        {
            int xvalue = 0;
            string sventa = "";
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0801", xcode);
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


            if (!string.IsNullOrEmpty(cliente))
            {

                ViewBag.cliente = cliente;
            }

            if (!string.IsNullOrEmpty(vendedor))
            {
                ViewBag.vendedor = vendedor;
            }

            if (!string.IsNullOrEmpty(documento))
            {
                ViewBag.documento = documento;
            }


            if (chkpendiente == 1)
            {
                ViewBag.chkpendiente = "Checked";
                sventa = string.Concat("0|");
            }

            if (chkparcial == 1)
            {
                ViewBag.chkparcial = "Checked";
                sventa = string.Concat(sventa, "1|");
            }

            if (chktotal == 1)
            {
                ViewBag.chktotal = "Checked";
                sventa = string.Concat(sventa,"2|");
            }
            if (chkpendiente == 0 && chkparcial == 0 && chktotal == 0)
            {
                sventa = string.Concat("0|", "1|", "2|");
            }

            ViewBag.fini = fini;
            ViewBag.ffin = ffin;

            var ORDEN_PEDIDOS = db.Pr_PedidoConsulta(1,0,fini,ffin,sventa,cliente,vendedor,documento).ToList();

            return View(ORDEN_PEDIDOS);
        }

        public async Task<ActionResult> Details(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0805", xcode);
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
            ORDEN_PEDIDOS pedido = await db.ORDEN_PEDIDOS.FindAsync(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        public async Task<ActionResult> OPDetalles()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0806", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

            var result = db.Pr_PedidoConsultaDetallada(0).ToList();

            return View(result);
        }

        // GET: ORDEN_PEDIDOS/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0802", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc == yfecha);
            if (result == null)
            {
                ViewBag.mensaje = "No se ha registrado el tipo de cambio, comuniquese con el administrador del sistema";
                return View("_Mensaje");
                //                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;
            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");
            ViewBag.poretencion = Helpers.Funciones.ObtenerValorParam("GENERAL", "% RETENCION");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked":"Unchecked";
            ViewBag.conf_articulosrepetidos = Helpers.Funciones.ObtenerValorParam("GENERAL", 1087);

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5 && c.ncode_confi == 1066), "ncode_confi", "sdesc_confi");
            ViewBag.sseri_orpe = new SelectList(db.Pr_DocSerie(1,User.Identity.Name,0,1066), "ncode_dose", "serie");
            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi",ViewBag.moneda);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ncode_vende = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_venzo", "VendeZona","");
            ViewBag.dfeorpeo_orpe = string.Format("{0:dd/MM/yyyy}", yfecha);
            ViewBag.dfevenci_orpe = string.Format("{0:dd/MM/yyyy}", yfecha);
            ViewBag.dfdespacho_orpe = string.Format("{0:dd/MM/yyyy}", yfecha);
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
            string xmensaje = "";
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
                                DateTime.Parse(mofView.sfevenci_orpe), DateTime.Parse(mofView.sfedespacho_orpe), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_orpe, mofView.ntc_orpe, mofView.ncode_fopago,
                                mofView.sobse_orpe, mofView.scode_compra, mofView.nbrutoex_orpe, mofView.nbrutoaf_orpe,
                                mofView.ndctoex_orpe, mofView.ndsctoaf_orpe, mofView.nsubex_orpe, mofView.nsubaf_orpe, mofView.nigvex_orpe,
                                mofView.nigvaf_orpe, mofView.ntotaex_orpe, mofView.ntotaaf_orpe, mofView.ntotal_orpe, mofView.ntotalMN_orpe,
                                mofView.ntotalUs_orpe, true, mofView.nvalIGV_orpe, User.Identity.Name, mofView.ncode_alma, int.Parse(User.Identity.GetLocal()), 
                                mofView.ncode_mone,mofView.ncode_vende,mofView.bclienteagretencion,mofView.ncode_dose,mofView.ncuotas_orpe,
                                mofView.ncuotavalor_orpe,mofView.ncuotadias_orpe,mofView.sglosadespacho_orpe,mofView.bflete_orpe,sw);


                            code = int.Parse(sw.Value.ToString());
                            if (code == -1)
                            {
                                xmensaje = "la numeracion ya ha sido utilizada, actualizar la numeraci+on ";
                                return Json(new { Success = 3, Mensaje = xmensaje });
                            }
                            //cccode = int.Parse(cc.Value.ToString());

                            if (mofView.ordenpedidoViewDetas != null)
                            {
                                foreach (ordenpedidoViewDeta item in mofView.ordenpedidoViewDetas)
                                {
                                    fila++;
                                    db.Pr_Orden_PedidoDetaCrea(code, item.ncode_arti, item.ncant_orpedeta, item.npu_orpedeta,
                                        item.ndscto_orpedeta, item.ndscto2_orpedeta, item.nexon_orpedeta, item.nafecto_orpedeta, 
                                        item.besafecto_orpedeta,item.ncode_alma, item.ndsctomax_orpedeta, 
                                        item.ndsctomin_orpedeta, item.ndsctoporc_orpedeta,item.npuorigen_orpedeta,item.npreciotope_orpedeta,fila);
                                };

                            }

                            if (mofView.ordenpedidoViewCuotas != null)
                            {
                                foreach (ordenpedidoViewCuota item in mofView.ordenpedidoViewCuotas)
                                {
                                    fila++;
                                    db.Pr_Orden_PedidoCuotaCrud("C", 0, code, DateTime.Parse(item.sfecharegistro), item.nvalor_orpecu, User.Identity.Name, sw);
                                        
                                };

                            }


                            db.Pr_KardexElimina("pedido", code);

                            db.Pr_KardexCrea("PEDIDO", 1039, "R", code, User.Identity.Name);
                            
                            Alert(1, "Orden de Pedido Registrada", NotificationType.success);
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

            db.Pr_PermisoAcceso(User.Identity.Name, "0803", xcode);
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
 
            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");
            ViewBag.poretencion = Helpers.Funciones.ObtenerValorParam("GENERAL", "% RETENCION");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";
            ViewBag.conf_articulosrepetidos = Helpers.Funciones.ObtenerValorParam("GENERAL", 1087);


            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", oRDEN_PEDIDOS.smone_orpe);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5 && c.ncode_confi == 1066), "ncode_confi", "sdesc_confi", oRDEN_PEDIDOS.ncode_docu);
            ViewBag.sseri_orpe = new SelectList(db.Pr_DocSerie(1, User.Identity.Name, 0, 1066), "ncode_dose", "serie",oRDEN_PEDIDOS.ncode_dose);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", oRDEN_PEDIDOS.ncode_alma);
            ViewBag.ncode_vende = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_venzo", "VendeZona", oRDEN_PEDIDOS.ncode_venzo);
            ViewBag.cod_cliente = oRDEN_PEDIDOS.ncode_cliente;
            ViewBag.sdesc_cliente = oRDEN_PEDIDOS.CLIENTE.srazon_cliente;
            ViewBag.sruc_cliente = oRDEN_PEDIDOS.CLIENTE.sruc_cliente;
            ViewBag.sdni_cliente = oRDEN_PEDIDOS.CLIENTE.sdnice_cliente;
            ViewBag.dfeorpeo_orpe = string.Format("{0:dd/MM/yyyy}", oRDEN_PEDIDOS.dfeorpeo_orpe);
            ViewBag.dfevenci_orpe = string.Format("{0:dd/MM/yyyy}", oRDEN_PEDIDOS.dfevenci_orpe);
            ViewBag.dfdespacho_orpe = string.Format("{0:dd/MM/yyyy}", oRDEN_PEDIDOS.dfdespacho_orpe);
            ViewBag.tc = oRDEN_PEDIDOS.ntc_orpe;
            
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
                                mofView.sobse_orpe, mofView.scode_compra, mofView.nbrutoex_orpe, mofView.nbrutoaf_orpe,
                                mofView.ndctoex_orpe, mofView.ndsctoaf_orpe, mofView.nsubex_orpe, mofView.nsubaf_orpe, mofView.nigvex_orpe,
                                mofView.nigvaf_orpe, mofView.ntotaex_orpe, mofView.ntotaaf_orpe, mofView.ntotal_orpe, mofView.ntotalMN_orpe,
                                mofView.ntotalUs_orpe,true, mofView.nvalIGV_orpe, User.Identity.Name, mofView.ncode_alma, 
                                int.Parse(User.Identity.GetLocal()), mofView.ncode_mone,mofView.ncode_vende,mofView.bclienteagretencion,
                                mofView.ncuotas_orpe,mofView.ncuotavalor_orpe, mofView.ncuotadias_orpe, mofView.sglosadespacho_orpe, mofView.bflete_orpe, sw);


                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_orpe;

                            if (mofView.ordenpedidoViewDetas != null)
                            {
                                foreach (ordenpedidoViewDeta item in mofView.ordenpedidoViewDetas)
                                {
                                    fila++;
                                    db.Pr_Orden_PedidoDetaCrea(code, item.ncode_arti, item.ncant_orpedeta, item.npu_orpedeta,
                                        item.ndscto_orpedeta, item.ndscto2_orpedeta, item.nexon_orpedeta, item.nafecto_orpedeta,
                                        item.besafecto_orpedeta,item.ncode_alma, item.ndsctomax_orpedeta, 
                                        item.ndsctomin_orpedeta, item.ndsctoporc_orpedeta,item.npuorigen_orpedeta,item.npreciotope_orpedeta,fila);
                                };

                            }

                            if (mofView.ordenpedidoViewCuotas != null)
                            {
                                db.Pr_Orden_PedidoCuotaCrud("D", 0, code, DateTime.Parse(mofView.sfeordenpedido_orpe), 0, "", sw);

                                foreach (ordenpedidoViewCuota item in mofView.ordenpedidoViewCuotas)
                                {
                                    fila++;
                                    db.Pr_Orden_PedidoCuotaCrud("C", 0, code, DateTime.Parse(item.sfecharegistro), item.nvalor_orpecu, User.Identity.Name, sw);

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

            db.Pr_PermisoAcceso(User.Identity.Name, "0804", xcode);
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

        public ActionResult Reporte(long id) {

            ORDEN_PEDIDOS _pedido = db.ORDEN_PEDIDOS.Single(x => x.ncode_orpe == id);

            return new ViewAsPdf("Reporte", _pedido);
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
