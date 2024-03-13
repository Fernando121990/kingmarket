using MarketASP.Extensiones;
using MarketASP.Clases;
using MarketASP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Collections;
using System.Web.WebPages;

namespace MarketASP.Controllers
{
    public class FuncionesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public JsonResult getTipoCambio(string accion,string sFecha)
        {
            //string accion = "venta";

            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_tipoCambioFecha(accion, sFecha).ToList();
            return Json(result);
        }

        public JsonResult getUbigeo(string sdescUbigeo)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.UBIGEO
                         where (s.sdistri_ubigeo + s.sprovi_ubigeo + s.sdepa_ubigeo).Contains(sdescUbigeo)
                         select new { codigo = s.scode_ubigeo, direccion = s.sdistri_ubigeo + " " + s.sprovi_ubigeo + " " + s.sdepa_ubigeo };
            return Json(result);
        }


        public JsonResult getCliente(string sdescCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_ClienteBusca(1, sdescCliente, "", "").ToList();
            return Json(result);
        }

        public JsonResult getRucCliente(string srucCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_ClienteBusca(1, "",srucCliente, "").ToList();
            return Json(result);
        }

        public JsonResult getDniCliente(string sdniCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_ClienteBusca(1, "", "",sdniCliente).ToList();
            return Json(result);
        }

        public JsonResult getDocuNumero(int ncode_dose)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_DocSerieNumero(ncode_dose).ToList();    //.Pr_DocNumeracion(1,User.Identity.Name, int.Parse(User.Identity.GetLocal()),ndocu).ToList();
            return Json(result);
        }
        public JsonResult getDocuSerie(int ncode_docu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_DocSerie(1, User.Identity.Name, int.Parse(User.Identity.GetLocal()), ncode_docu).ToList();
            return Json(result);
        }

        public JsonResult getDiasFormaPago(int ncode_fopago)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.CONFIGURACION
                         where (s.ncode_confi == ncode_fopago)
                         select new { dias = s.svalor_confi };
            return Json(result);
        }

        public JsonResult getProveedor(string sdescProvee)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = from s in db.PROVEEDOR
                         where s.sdesc_prove.Contains(sdescProvee)
                         select new { s.ncode_provee, s.sdesc_prove,s.sruc_prove};
            return Json(result);
        }

        public JsonResult getClienteDire(Int32 scodCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            //var result = from s in db.CLI_DIRE
            //             where s.ncode_cliente.Equals(scodCliente)
            //             select new { s.ncode_clidire, s.sdesc_clidire };
            var result = db.Pr_ClienteDirecciones(scodCliente);
            return Json(result);
        }

        public JsonResult getClienteFoPago(Int32 scodCliente)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_clienteFormaPagos(scodCliente);
            return Json(result);
        }
        public JsonResult getKardex(int ncode_arti)
        {
            var resultado = db.Pr_KardexArticulos(ncode_arti,"","",0);

            return Json(resultado);
        }
        public JsonResult getStockDisponible(int ncode_arti, int ncode_alma)
        {
            try
            {
                var resultado = db.Pr_KardexArticulos(ncode_arti, "", "", ncode_alma).ToList();


                decimal resudisponible = 0;

                if (resultado != null && resultado.Count > 0)
                {
                    var xstock = resultado.ToArray();
                    resudisponible = (decimal)xstock[0].STOCK;
                }

                return Json(resudisponible);

            }
            catch (Exception)
            {

                return Json(0);
            }
        }

        public JsonResult getLotesDisponible(int ncode_alma,int ncode_arti,string fvenci_lote,string sdesc_lote) {
            var resultado = db.Pr_LotesDisponibles(ncode_alma,ncode_arti,fvenci_lote,sdesc_lote);
            return Json(resultado);
        }
        
        public JsonResult getPedidoPrecio(int ncode_arti)
        {
            var resultado = db.Pr_PedidoPrecio(ncode_arti);

            return Json(resultado);
        }
        public JsonResult getGuiaVenta()
        {
            var resultado = db.Pr_GuiaVentaLista(2, "", "");
            return Json(resultado);
        }


        public JsonResult getPedidoVenta()
        {
            var resultado = db.Pr_OrdenPedidoLista(2,"","");
            return Json(resultado);
        }
        public JsonResult getPedidoCompra()
        {
            var resultado = db.Pr_OrdenCompraLista(2, "", "");
            return Json(resultado);
        }

        public JsonResult getOCompraPrecio(int ncode_arti)
        {
            var resultado = db.Pr_OCompraPrecio(ncode_arti);

            return Json(resultado);
        }

        public JsonResult getArticulosLotes(int ncode_compra)
        {
            var resultado = db.Pr_CompraLotesArticulo(ncode_compra);

            return Json(resultado);
        }

        public JsonResult getArticulos()
        {
            var result = db.Pr_ConsultaArticulos();

            return Json(result); 
        }
        public JsonResult getMatTabla(int ncode_alma)
        {
            var result = db.Pr_KardexMatTabla(0, ncode_alma);

            return Json(result);
        }


        public JsonResult getProforma(string snume)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_ProformaLista(snume, "");
            return Json(result);
        }

        public JsonResult getProfoVenta(Int32 ncode_prof)
        {
            db.Configuration.ProxyCreationEnabled = false;
            PROFORMAS pROFORMAS = db.PROFORMAS.Find(ncode_prof);
            CLIENTE cLIENTE = db.CLIENTE.Find(pROFORMAS.ncode_cliente);
            List<PROFORMA_DETALLE> lista = db.PROFORMA_DETALLE.Include("ARTICULO").Where(p => p.ncode_prof == ncode_prof).ToList();
            List<ventaViewDeta> listadeta = new List<ventaViewDeta>();

            foreach (var item in lista)
            {
                ventaViewDeta deta = new ventaViewDeta
                {
                    besafecto_vedeta = item.besafecto_profdeta,
                    bisc_vedeta = item.ARTICULO.bisc_arti,
                    scod2 = item.ARTICULO.scode_arti,
                    sdesc = item.ARTICULO.sdesc1_arti,
                    nafecto_vedeta = item.nafecto_profdeta,
                    ncant_vedeta = item.ncant_profdeta,
                    ncode_arti = item.ncode_arti,
                    npu_vedeta = item.npu_profdeta,
                    ndscto_vedeta = item.ndscto_profdeta,
                    nexon_vedeta = item.nexon_profdeta
                };
                listadeta.Add(deta);
            }


            ventaView ventaView = new ventaView
            {
                ncode_cliente = pROFORMAS.ncode_cliente,
                ncode_clidire = pROFORMAS.ncode_clidire,
                scliente = cLIENTE.srazon_cliente,
                sruc = cLIENTE.sruc_cliente,
                sdni = cLIENTE.sdnice_cliente,
                ncode_fopago = pROFORMAS.ncode_fopago,
                ncode_mone = pROFORMAS.ncode_mone,
                ventaViewDetas = listadeta
            };

            return Json(ventaView);
        }

        //public JsonResult getOrdenPedidoVenta(Int32 ncode_orpe)
        public JsonResult getOrdenPedidoVenta(string scode_orpe,string documentos)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var orpe = scode_orpe.Split('|');
            long ncode_orpe = long.Parse(orpe[0]);
            ORDEN_PEDIDOS pedido = db.ORDEN_PEDIDOS.Find(ncode_orpe);
            CLIENTE cLIENTE = db.CLIENTE.Find(pedido.ncode_cliente);
            var lista = db.Pr_Orden_PedidoDetVentaGuia(scode_orpe).ToList();

            List<ORDEN_PEDIDOS_CUOTAS> listacuotaop = db.ORDEN_PEDIDOS_CUOTAS.Where(p => p.ncode_orpe == ncode_orpe).ToList();
            List<ventaViewDeta> listadeta = new List<ventaViewDeta>();
            List<ventaViewCuota> listacuota = new List<ventaViewCuota>();

            foreach (var item in listacuotaop)
            {
                ventaViewCuota deta = new ventaViewCuota
                {
                    ncode_vedecu = item.ncode_orpecu,
                    dfreg_vedecu = item.dfreg_orpecu,
                    nvalor_vedecu = item.nvalor_orpecu,
                    sfecharegistro = string.Format("{0:d}",item.dfreg_orpecu)
                };
                listacuota.Add(deta);
            }



            foreach (var item in lista)
            {
                ventaViewDeta deta = new ventaViewDeta
                {
                    
                    besafecto_vedeta = item.besafecto_orpedeta,
                    bisc_vedeta = item.bisc_arti,
                    scod2 = item.scode_arti,
                    sdesc = item.sdesc1_arti,
                    nafecto_vedeta = item.nafecto_orpedeta,
                    ncant_vedeta = item.ncantventa_orpedeta,
                    ncode_arti = item.ncode_arti,
                    npu_vedeta = item.npu_orpedeta,
                    ndscto_vedeta = item.ndscto_orpedeta,
                    nexon_vedeta = item.nexon_orpedeta,
                    sumed = item.ssunat_umed,
                    ncode_umed = item.ncode_umed,
                    blote_vedeta = item.blote_arti,
                    bdescarga_vedeta = item.bdescarga,
                    ncode_orpe =  item.ncode_orpe
                };
                listadeta.Add(deta);
            }


            ventaView ventaView = new ventaView
            {
                ncode_cliente = pedido.ncode_cliente,
                ncode_clidire = pedido.ncode_clidire,
                scliente = cLIENTE.srazon_cliente,
                sruc = cLIENTE.sruc_cliente,
                sdni = cLIENTE.sdnice_cliente,
                ncode_fopago = pedido.ncode_fopago,
                ncode_mone = pedido.ncode_mone,
                ncode_alma = pedido.ncode_alma,
                bclienteagretencion = pedido.bclienteagretencion,
                ncuotas_venta = pedido.ncuotas_orpe,
                ncuotadias_venta = pedido.ncuotadias_orpe,
                ncuotavalor_venta = pedido.ncuotavalor_orpe,
                sglosadespacho_venta = pedido.sglosadespacho_orpe,
                sobse_venta = pedido.sobse_orpe,
                scode_compra = pedido.scode_compra,
                sfeventa_venta = string.Format("{0:d}", pedido.dfeorpeo_orpe),
                sfevenci_venta = string.Format("{0:d}", pedido.dfevenci_orpe),
                bflete_venta = pedido.bflete_orpe,
                nretencionvalor_venta = 0,
                ncode_venzo = pedido.ncode_venzo,
                sserienumero = documentos, ///string.Concat(pedido.sseri_orpe,"-",pedido.snume_orpe),
                ventaViewDetas = listadeta,
                ventaViewCuotas = listacuota
            };

            return Json(ventaView);
        }

        public JsonResult getGuiaVentaAsociar(string scode_guia, string documentos)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var lguia = scode_guia.Split('|');
            long ncode_guia = long.Parse(lguia[0]);

            GUIA pedido = db.GUIA.Find(ncode_guia);
            CLIENTE cLIENTE = db.CLIENTE.Find(pedido.ncode_cliente);
            var lista = db.Pr_GuiaVentaDetVentaGuia(scode_guia).ToList();
            List<ventaViewDeta> listadeta = new List<ventaViewDeta>();

            foreach (var item in lista)
            {
                ventaViewDeta deta = new ventaViewDeta
                {
                    besafecto_vedeta = (bool)item.bafecto_arti,
                    bisc_vedeta = item.bisc_arti,
                    scod2 = item.scode_arti,
                    sdesc = item.sdesc1_arti,
                    nafecto_vedeta = item.nafecto_guiadet,
                    ncant_vedeta = item.ncant_guiadet,
                    ncode_arti = item.ncode_arti,
                    npu_vedeta = item.npu_guiadet,
                    ndscto_vedeta = item.ndscto_guiadet,
                    nexon_vedeta = item.nexon_guiadet,
                    sumed = item.ssunat_umed,
                    ncode_umed = item.ncode_umed,
                    blote_vedeta = item.blote_arti,
                    bdescarga_vedeta = item.bdescarga_arti
                };
                listadeta.Add(deta);
            }


            ventaView ventaView = new ventaView
            {
                ncode_cliente = (int)pedido.ncode_cliente,
                ncode_clidire = pedido.ncode_clidire,
                scliente = cLIENTE.srazon_cliente,
                sruc = cLIENTE.sruc_cliente,
                sdni = cLIENTE.sdnice_cliente,
                ncode_fopago =  (int) pedido.ncode_fopago,
                ncode_mone = pedido.ncode_mone,
                ncode_alma = pedido.ncode_alma,
                ncode_venzo = pedido.ncode_venzo,
                bclienteagretencion = pedido.bclienteagretencion,
                ncuotas_venta = pedido.ncuotas_guia,
                ncuotadias_venta = pedido.ncuotadias_guia,
                ncuotavalor_venta = pedido.ncuotavalor_guia,
                sglosadespacho_venta = pedido.sglosadespacho_guia,
                sobse_venta = pedido.sobse_guia,
                bflete_venta = pedido.bflete_guia,
                nretencionvalor_venta = 0,
                sserienumero = string.Concat(pedido.sserie_guia, "-", pedido.snume_guia),
                scode_compra = pedido.scode_compra,
                ventaViewDetas = listadeta
            };

            return Json(ventaView);
        }

        public JsonResult getOrdenCompra(Int32 ncode_orco)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ORDEN_COMPRAS pedido = db.ORDEN_COMPRAS.Find(ncode_orco);
            PROVEEDOR pROVEEDOR = db.PROVEEDOR.Find(pedido.ncode_provee);
            var lista = db.Pr_Orden_CompraDetCompra(ncode_orco).ToList();
            List<compraViewDeta> listadeta = new List<compraViewDeta>();

            foreach (var item in lista)
            {
                compraViewDeta deta = new compraViewDeta
                {

                    besafecto_comdeta = item.besafecto_orcodeta,
                    bisc_comdeta = false,
                    scod2 = item.scode_arti,
                    sdesc = item.sdesc1_arti,
                    sund = item.ssunat_umed,
                    nafecto_comdeta = item.nafecto_orcodeta,
                    ncant_comdeta = item.ncantventa_orcodeta,
                    ncode_arti = item.ncode_arti,
                    npu_comdeta = item.npu_orcodeta,
                    ndscto_comdeta = item.ndscto_orcodeta,
                    nexon_comdeta = item.nexon_orcodeta,
                    
                };
                listadeta.Add(deta);
            }


            compraView _compraView = new compraView
            {
                ncode_provee = (long)pedido.ncode_provee,
                sproveedor = pROVEEDOR.sdesc_prove,
                sruc = pROVEEDOR.sruc_prove,
                ncode_fopago = pedido.ncode_fopago,
                smone_compra = pedido.smone_orco,
                ntipo_orco = pedido.ncode_docu,
                stipo_orco = pedido.stipo_orco,
                sserie_orco = string.Concat(pedido.sseri_orco,"-",pedido.snume_orco),
                ncode_alma = pedido.ncode_alma,
                sfecompra_compra = string.Format("{0:d}", pedido.dfeorco_orco),
                sfevenci_compra = string.Format("{0:d}", pedido.dfevenci_orco),
                compraViewDetas = listadeta
            };

            return Json(_compraView);
        }
        public JsonResult getReceta(Int32 ncode_receta)
        {
            db.Configuration.ProxyCreationEnabled = false;
            
            Receta receta = db.Receta.Find(ncode_receta);
            List<RecetaDetalle> lista = db.RecetaDetalle.Include("ARTICULO")
                .Where(p => p.Rec_codigo == ncode_receta).ToList();

           List<genericoDetalle> listadeta = new List<genericoDetalle>();

            foreach (var item in lista)
            {
                var medida = db.UMEDIDA.SingleOrDefault(x => x.ncode_umed == item.ARTICULO.ncode_umed);
                genericoDetalle deta = new genericoDetalle();
                {
                    deta.sdescripcion = item.ARTICULO.sdesc1_arti;
                    deta.ncodigo = item.ARTICULO.ncode_arti;
                    deta.scodigo = item.ARTICULO.scode_arti;
                    deta.ncantidad = item.RecD_Cantidad;
                    deta.sumedida = medida.sdesc_umed;
                    deta.numedida = item.ARTICULO.ncode_umed;
                    deta.nalmacen = item.ncode_alma;
                    deta.nprecio = item.RecD_precio;

                };
                listadeta.Add(deta);
            }

            genericoCabecera cabecera = new genericoCabecera { 
                codigo = receta.Rec_codigo,
                cantidad = receta.Rec_cantidad,
                costoOperativo = receta.Rec_costoOperativo,
                detalle = listadeta,
            };

            return Json(cabecera);
        }
        public string fncadenaeditar(string id, string value, int column)
        {

            if (value.IsFloat())
            {
                value = string.Format("{0:N4}", value);
            }

            return value;
        }
        public decimal fnnumeroeditar(string id, decimal value, int column)
        {
            value = Math.Round(value,4);
            return value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateCliente(string model_json)
        {
            ObjectParameter xcod = new ObjectParameter("xcod", typeof(String));
            ObjectParameter ingproc_001 = new ObjectParameter("ingproc_001", typeof(long));
            string data = "";
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
                        var mofView = JsonConvert.DeserializeObject<clienteView>(data, jsonSettings);

                        if (mofView != null)
                        {
                            //VERIFICAR RUC O DNI
                            clienteView cLIENTE = mofView;

                            if (cLIENTE.stipo_cliente == "J")
                            {
                                CLIENTE xcli = db.CLIENTE.SingleOrDefault(c => c.sruc_cliente == cLIENTE.sruc_cliente);
                                if (xcli != null)
                                {
                                    //ViewBag.mensaje = "El cliente ya existe";
                                    return Json(new { Success = 0, mensaje = "El cliente ya existe" });
                                }

                            }

                            if (cLIENTE.stipo_cliente == "N")
                            {
                                CLIENTE xcli = db.CLIENTE.SingleOrDefault(c => c.sdnice_cliente == cLIENTE.sdnice_cliente);
                                if (xcli != null)
                                {
                                    return Json(new { Success = 0, mensaje = "El cliente ya existe" });
                                }

                            }

                            CLIENTE cliente = ToCliente(cLIENTE);

                            db.CLIENTE.Add(cliente);
                            db.SaveChangesAsync();

                            int id = cliente.ncode_cliente;
                            CLI_DIRE cLI_DIRE = toCliDire(cLIENTE, id);

                            db.CLI_DIRE.Add(cLI_DIRE);
                            db.SaveChangesAsync();

                            ///return RedirectToAction("Index");

                            //db.MANT_CLIENTES("", mofView.srazon_cliente, mofView.sruc_cliente, mofView.sdni_cliente, mofView.sfono_cliente, "", 0, mofView.srep_cliente
                            //    , mofView.semail_cliente, "", true, "", mofView.sfono2_cliente, "", "", 0, "W", "", "", "", "", "", 0, mofView.ape_pat_cliente,
                            //    mofView.ape_mat_cliente, mofView.nombres_cliente, mofView.tipo_contribuyente, ingproc_001, xcod);

                            //code = xcod.Value.ToString();

                            //db.GRABAR_DIR_CLIENTE(code, mofView.sdire_cliente, mofView.subigeo_cliente, "", 0, ingproc_001);

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

        private CLI_DIRE toCliDire(clienteView cLIENTE, int id)
        {
            return new CLI_DIRE
            {
                scode_ubigeo = cLIENTE.scode_ubigeo,
                sdesc_clidire = cLIENTE.sdire_cliente,
                ncode_cliente = id
            };
        }

        private CLIENTE ToCliente(clienteView cliView)
        {
            return new CLIENTE
            {
                bprocedencia_cliente = cliView.bprocedencia_cliente,
                dfech_cliente = DateTime.Now,
                sapma_cliente = cliView.sapma_cliente,
                sappa_cliente = cliView.sappa_cliente,
                sdnice_cliente = cliView.sdnice_cliente,
                sfax_cliente = cliView.sfax_cliente,
                sfono1_cliente = cliView.sfono1_cliente,
                sfono2_cliente = cliView.sfono2_cliente,
                sfono3_cliente = cliView.sfono3_cliente,
                slineacred_cliente = cliView.slineacred_cliente,
                smail_cliente = cliView.smail_cliente,
                snomb_cliente = cliView.snomb_cliente,
                sobse_cliente = cliView.sobse_cliente,
                srazon_cliente = (cliView.stipo_cliente == "J") ? cliView.srazon_cliente : cliView.snomb_cliente + " " + cliView.sappa_cliente + " " + cliView.sapma_cliente,
                srepre_cliente = cliView.srepre_cliente,
                sruc_cliente = cliView.sruc_cliente,
                stipo_cliente = cliView.stipo_cliente,
                suser_cliente = User.Identity.Name,
                sweb_cliente = cliView.sweb_cliente,
            };

        }

        

    }
}