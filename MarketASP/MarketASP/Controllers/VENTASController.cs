using MarketASP.Clases;
using MarketASP.Models;
using MarketASP.Extensiones;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System.IO;
using SunatService.Modelos;

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

            //var vENTAS = db.VENTAS.Include(v => v.CLI_DIRE).Include(v => v.CLIENTE).Include(v => v.CONFIGURACION).Include(v => v.CONFIGURACION1);
            var result = db.Pr_VentaListado(1,"","").ToList();
            return View(result);
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
                ViewBag.mensaje = "No se ha registrado el tipo de cambio, comuniquese con el administrador del sistema";
                return View("_Mensaje");
//                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }
            ViewBag.tc = result.nventa_tc;
            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.smone_venta = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi",ViewBag.moneda);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ncode_vende = new SelectList(db.Pr_VendedoresLista(0).Where(c => c.nesta_vende == true), "ncode_vende", "VendeZona", "");
            ViewBag.dfeventa_venta = string.Format("{0:dd/MM/yyyy}", yfecha);
            ViewBag.dfevenci_venta = string.Format("{0:dd/MM/yyyy}", yfecha);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter cc = new ObjectParameter("cc", typeof(int));
            SunatResponse resultado = new SunatResponse();
            string xmensaje = "";
            int code = 0;
            int cccode = 0;
            string data = "";
            int fila = 0;
            int xcontado = 0;
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
                            xcontado = mofView.ncode_fopago;
                            //verificar stock
                            var bstock = true;

                            if (mofView.ventaViewDetas != null)
                            {
                                foreach (ventaViewDeta item in mofView.ventaViewDetas)
                                {
                                    fila++;

                                    var rstock = db.Pr_KardexArticulos(item.ncode_arti, "", "", item.ncode_alma).ToList();
                                    
                                    decimal cantdisponible = 0 ;

                                    if (rstock != null && rstock.Count > 0)
                                    {
                                        var xstock = rstock.ToArray();
                                        cantdisponible = (decimal) xstock[0].STOCK;
                                    }


                                    if (cantdisponible < item.ncant_vedeta) {
                                        xmensaje += string.Format("{0}{1}",item.sdesc," - ");
                                        bstock = false;
                                    }
                                };

                            }

                            if (!bstock)
                            {
                                xmensaje = "Verificar el stock de producto de los articulos " + xmensaje;
                                return Json(new { Success = 3, Mensaje = xmensaje });
                            }


                            db.Pr_ventaCrea(mofView.ncode_docu, mofView.sseri_venta, mofView.snume_venta, DateTime.Parse(mofView.sfeventa_venta),
                                DateTime.Parse(mofView.sfevenci_venta), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_venta, mofView.ntc_venta, mofView.ncode_fopago,
                                mofView.sobse_venta, mofView.scode_compra, mofView.ncode_profo, mofView.nbrutoex_venta, mofView.nbrutoaf_venta,
                                mofView.ndctoex_venta, mofView.ndsctoaf_venta, mofView.nsubex_venta, mofView.nsubaf_venta, mofView.nigvex_venta,
                                mofView.nigvaf_venta, mofView.ntotaex_venta, mofView.ntotaaf_venta, mofView.ntotal_venta, mofView.ntotalMN_venta,
                                mofView.ntotalUs_venta,mofView.nicbper_venta, true, mofView.nvalIGV_venta, User.Identity.Name,mofView.ncode_alma,int.Parse(User.Identity.GetLocal()),mofView.ncode_mone,
                                ConfiguracionSingleton.Instance.glbcobroAutomatico,mofView.ncode_vende,mofView.ncode_orpe, sw,cc);


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

                            //verificar si la venta esta asociada a una orden de pedido
                             //var sconfi = Helpers.Funciones.ObtenerValorParam("GENERAL", "VENTA X PEDIDO");
                            //if (sconfi == "SI")
                            if (ConfiguracionSingleton.Instance.glbVentaxPedido == "SI" && mofView.ncode_orpe != null && mofView.ncode_orpe > 0)
                            {
                                db.Pr_KardexCrea("Venta", 5, "R", code, User.Identity.Name);
                            }
                            

                            db.Pr_ventaActualizaPedido(0,mofView.ncode_orpe, code);

                            resultado.Success = true;

                            //inicio el proceso de envio a sunat
                            //try
                            //{
                            //    resultado = EnviarComprobante(mofView);
                            //}
                            //catch (Exception ex)
                            //{

                            //    resultado.Error.Description = "EnvSuna" + ex.Message;
                            //    return Json(new { Success = 3, Mensaje = resultado.Error.Description });
                            //}
                        }
                    }
                }

                if (resultado.Success)
                {
                    xmensaje = "Registro exitoso en Sunat y Local - " + resultado.info;
                }
                else
                {
                    xmensaje = "Error Sunat - " + resultado.Error.Description;
                }

                if (ConfiguracionSingleton.Instance.glbContado != xcontado)
                {
                    return Json(new { Success = 1, Mensaje = xmensaje });
                }

                if (ConfiguracionSingleton.Instance.glbcobroAutomatico == "SI")
                {
                    return Json(new { Success = 2, CtaCo = cccode, Mensaje = xmensaje });
                }


                return Json(new { Success = 1, Mensaje = xmensaje });

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
                mensaje = "Venta" + mensaje + xmensaje;
                return Json(new { Success = 3, Mensaje = mensaje });
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

            ViewBag.cod_cliente = vENTAS.ncode_cliente;
            ViewBag.sdesc_cliente = vENTAS.CLIENTE.srazon_cliente;
            ViewBag.sruc_cliente = vENTAS.CLIENTE.sruc_cliente;
            ViewBag.sdni_cliente = vENTAS.CLIENTE.sdnice_cliente;
            ViewBag.tc = vENTAS.ntc_venta;
            ViewBag.dfeventa_venta = string.Format("{0:dd/MM/yyyy}", vENTAS.dfeventa_venta);  
            ViewBag.dfevenci_venta = string.Format("{0:dd/MM/yyyy}", vENTAS.dfevenci_venta);



            ViewBag.ncode_vende = new SelectList(db.Pr_VendedoresLista(0).Where(c => c.nesta_vende == true), "ncode_vende", "VendeZona", vENTAS.ncode_vende);
            ViewBag.NRO_DCLIENTE = new SelectList(db.CLI_DIRE.Where(c => c.ncode_cliente == vENTAS.ncode_cliente), "ncode_clidire", "sdesc_clidire", vENTAS.ncode_clidire);
            return View(vENTAS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            
            string xmensaje = "";
            long code = 0;
            int cccode = 0;
            string data = "";
            int fila = 0;
            int xcontado = 0;
            int xsw;

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

                            xcontado = mofView.ncode_fopago;
                            //verificar stock
                            var bstock = true;

                            if (mofView.ventaViewDetas != null)
                            {
                                foreach (ventaViewDeta item in mofView.ventaViewDetas)
                                {
                                    fila++;

                                    var rstock = db.Pr_KardexArticulos(item.ncode_arti, "", "", item.ncode_alma).ToList();

                                    decimal cantdisponible = 0;

                                    if (rstock != null && rstock.Count > 0)
                                    {
                                        var xstock = rstock.ToArray();
                                        cantdisponible = (decimal)xstock[0].STOCK;
                                    }


                                    if (cantdisponible < item.ncant_vedeta)
                                    {
                                        xmensaje += string.Format("{0}{1}", item.sdesc, " - ");
                                        bstock = false;
                                    }
                                };

                            }

                            if (!bstock)
                            {
                                xmensaje = "Verificar el stock de producto de los articulos " + xmensaje;
                                return Json(new { Success = 3, Mensaje = xmensaje });
                            }


                            db.Pr_ventaEdita(mofView.ncode_venta, mofView.ncode_docu, DateTime.Parse(mofView.sfeventa_venta),
                                DateTime.Parse(mofView.sfevenci_venta), mofView.ncode_cliente, mofView.ncode_clidire, mofView.smone_venta, mofView.ntc_venta, mofView.ncode_fopago,
                                mofView.sobse_venta, mofView.scode_compra, mofView.ncode_profo, mofView.nbrutoex_venta, mofView.nbrutoaf_venta,
                                mofView.ndctoex_venta, mofView.ndsctoaf_venta, mofView.nsubex_venta, mofView.nsubaf_venta, mofView.nigvex_venta,
                                mofView.nigvaf_venta, mofView.ntotaex_venta, mofView.ntotaaf_venta, mofView.ntotal_venta, mofView.ntotalMN_venta,
                                mofView.ntotalUs_venta, mofView.nvalIGV_venta, User.Identity.Name, mofView.ncode_alma, 
                                int.Parse(User.Identity.GetLocal()),mofView.ncode_mone,mofView.ncode_vende, sw);


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

                            db.Pr_ventaDetaEdita(code);

                            db.Pr_KardexElimina("venta", code);

                            db.Pr_KardexCrea("Venta", 5, "S", code, User.Identity.Name);

                            
                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Registro Actualizado" });

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

        public SunatResponse EnviarComprobante(ventaView venta)
        {

            //Crear una clase cabecera de la capa SUNAT SERVICES
            SunatService.ViewModels.Cabecera comprobante = new SunatService.ViewModels.Cabecera();
            //Cargamos las propiedades de la clase cabecera de la capa sunat services
            comprobante.Idcabecera = 0; // venta.ncode_venta; 
            comprobante.Fechaemision = DateTime.Parse(venta.sfeventa_venta);
            comprobante.Fechavencimiento = DateTime.Parse(venta.sfevenci_venta);
            comprobante.Idtipocomp = db.CONFIGURACION.Where(C => C.ncode_confi == venta.ncode_docu).SingleOrDefault().svalor_confi; // venta.CONFIGURACION.svalor_confi; // .Idtipocomp.ToString().PadLeft(2, '0'); 
            comprobante.Serie = venta.sseri_venta;
            comprobante.Numero = venta.snume_venta;
            comprobante.Idcliente = venta.ncode_cliente;
            //Obtener datos del cliente mediante una consulta
            var cliente = db.CLIENTE.Where(p => p.ncode_cliente == comprobante.Idcliente).SingleOrDefault();
            var clidire = db.CLI_DIRE.Where(p => p.ncode_clidire == venta.ncode_clidire).SingleOrDefault();
            comprobante.ClienteRazonSocial = cliente.srazon_cliente;
            comprobante.ClienteDireccion = clidire.sdesc_clidire;
            comprobante.ClienteUbigeo = clidire.UBIGEO.scode_ubigeo;
            comprobante.ClienteTipodocumento = "6"; // cliente.IdTipoDoc; 
            comprobante.ClienteNumeroDocumento = cliente.sruc_cliente;
            //Cargamos los totales del comprobante
            comprobante.Igv = decimal.Parse(Helpers.Funciones.ObtenerValorParam("GENERAL","IGV"))/100;
            comprobante.TotSubtotal = (decimal) (venta.nsubaf_venta + venta.nsubex_venta); /// .TotSubtotal);
            comprobante.TotDsctos = venta.ndsctoaf_venta + venta.ndctoex_venta; // .TotDsctos;
            comprobante.TotIcbper = 0; // venta.TotIcbper; 
            comprobante.TotIgv = venta.nigvaf_venta + venta.nigvex_venta; //.TotIgv;
            comprobante.TotISC = 0; // venta.TotIsc;
            comprobante.TotOtros = 0; // venta.TotOtros;
            comprobante.TotTotal = comprobante.TotSubtotal - comprobante.TotDsctos;
            //comprobante.TotTributos = venta.TotIgv + venta.TotIsc + venta.TotOtros + venta.TotIcbper;
            comprobante.TotTributos = comprobante.TotIgv + comprobante.TotISC + comprobante.TotOtros;
            comprobante.TotNeto = venta.ntotal_venta + comprobante.TotTributos;
            comprobante.Total = venta.ntotal_venta + comprobante.TotTributos;
            comprobante.Idmoneda = "PEN"; // venta.ncode_mone; // .Idmoneda;

            //Capturar los datos de la empresa emisora
            //Captiurar datos del ubigeo de la empresa remitente
            var xlocal = User.Identity.GetLocal();
            int ylocal = int.Parse(xlocal);

            var empresa = db.LOCAL.Where(l => l.ncode_local == ylocal).SingleOrDefault();
            string ubigeo = empresa.scode_ubigeo;   //Util.Utiles.ObtenerValorParam("EMPRESA", "UBIGEO");
            comprobante.EmpresaDepartamento = ubigeo.Substring(0, 2);
            comprobante.EmpresaProvincia = ubigeo.Substring(2, 2);
            comprobante.EmpresaDistrito = ubigeo.Substring(4, 2);
            comprobante.EmpresaDepartamento = empresa.UBIGEO.sdepa_ubigeo; // Util.Utiles.ObtenerValorParam("EMPRESA", "DEPARTAMENTO_NOMBRE");
            comprobante.ID_EmpresaDepartamento = ubigeo.Substring(0, 2); // Util.Utiles.ObtenerValorParam("EMPRESA", "DEPARTAMENTO");
            comprobante.EmpresaProvincia = empresa.UBIGEO.sprovi_ubigeo; // Util.Utiles.ObtenerValorParam("EMPRESA", "PROVINCIA_NOMBRE");
            comprobante.ID_EmpresaProvincia = ubigeo.Substring(2, 2); // Util.Utiles.ObtenerValorParam("EMPRESA", "PROVINCIA");
            comprobante.EmpresaDistrito = empresa.UBIGEO.sdistri_ubigeo; // Util.Utiles.ObtenerValorParam("EMPRESA", "DISTRITO_NOMBRE");
            comprobante.ID_EmpresaDistrito = ubigeo.Substring(4, 2); // Util.Utiles.ObtenerValorParam("EMPRESA", "DISTRITO");
            //Capturar datos de empresa
            comprobante.EmpresaRazonSocial = empresa.SUCURSAL.sdesc_sucu; // Util.Utiles.ObtenerValorParam("EMPRESA", "NOMBRECOMERCIAL");
            comprobante.EmpresaDireccion = empresa.sdire_local; // Util.Utiles.ObtenerValorParam("EMPRESA", "DIRECCION");
            comprobante.EmpresaRUC = empresa.SUCURSAL.sruc_sucu; // Util.Utiles.ObtenerValorParam("EMPRESA", "RUC");

            //Grabar los detalles 
            List<SunatService.ViewModels.Detalles> details = new List<SunatService.ViewModels.Detalles>();
            foreach (var item in venta.ventaViewDetas)
            {
                SunatService.ViewModels.Detalles det = new SunatService.ViewModels.Detalles();
                var articulo = db.ARTICULO.Where(a => a.ncode_arti == item.ncode_arti).SingleOrDefault();
                det.Cantidad = (decimal) item.ncant_vedeta;
                det.Codcom = articulo.scode_arti;
                det.DescripcionProducto = articulo.sdesc1_arti;
                det.Precio = (decimal) item.npu_vedeta;
                det.Total = (decimal) item.nafecto_vedeta;
                det.UnidadMedida = "NIU";
                det.porIgvItem = Convert.ToDecimal(comprobante.Igv);
                det.mtoValorVentaItem = det.Total;
                details.Add(det);
            }
            comprobante.Detalles = details;

            //Enviar a SUNAT
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            SunatService.Servicios.SUNAT_UTIL servicioSUNAT = new SunatService.Servicios.SUNAT_UTIL();

            //Obtner la ruta de la aplicacion
            string RutaAplicacion = ControllerContext.HttpContext.Server.MapPath("/"); // _hostingEnvironment.WebRootPath;

            string pathsunat = ControllerContext.HttpContext.Server.MapPath("/");

            string RutaCertificado = Helpers.Funciones.ObtenerValorParam("RUTA","CERTIFICADO");
            string RutaXML = Helpers.Funciones.ObtenerValorParam("RUTA", "XML");
            string RutaCDR = Helpers.Funciones.ObtenerValorParam("RUTA", "CDR");
            string RutaEnvio = Helpers.Funciones.ObtenerValorParam("RUTA", "ENVIO");
            string RutaQR = Helpers.Funciones.ObtenerValorParam("RUTA", "QR");
            //Enviar las credenciales
            servicioSUNAT.Ruta_Certificado = Path.Combine(RutaAplicacion, RutaCertificado);
            servicioSUNAT.Password_Certificado = "123456";

            servicioSUNAT.Ruta_XML = Path.Combine(RutaAplicacion, RutaXML);
            servicioSUNAT.Ruta_ENVIOS = Path.Combine(RutaAplicacion, RutaEnvio);
            servicioSUNAT.Ruta_CDRS = Path.Combine(RutaAplicacion, RutaCDR);
            //servicioSUNAT.Ruta_QR = Path.Combine(RutaAplicacion, RutaQR);

            SunatResponse resultado = new SunatResponse();
            try
            {
                resultado = servicioSUNAT.GenerarComprobanteFB_XML(comprobante);
            }
            catch (Exception ex)
            {
                resultado.Error.Description = "GenCom " + ex.InnerException.Message;
                throw new Exception("EVCom " + ex.Message);
            }
            return resultado;
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
