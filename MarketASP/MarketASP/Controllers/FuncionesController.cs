﻿using MarketASP.Extensiones;
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

        public JsonResult getDocuNumero(int ndocu)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var result = db.Pr_DocNumeracion(1,User.Identity.Name, int.Parse(User.Identity.GetLocal()),ndocu).ToList();
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

        public JsonResult getOrdenPedidoVenta(Int32 ncode_orpe)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ORDEN_PEDIDOS pedido = db.ORDEN_PEDIDOS.Find(ncode_orpe);
            CLIENTE cLIENTE = db.CLIENTE.Find(pedido.ncode_cliente);
            List<ORDEN_PEDIDOS_DETALLE> lista = db.ORDEN_PEDIDOS_DETALLE.Include("ARTICULO")
                .Where(p => p.ncode_orpe == ncode_orpe).Where(p=> p.ncantventa_orpedeta > 0).ToList();
            List<ventaViewDeta> listadeta = new List<ventaViewDeta>();

            foreach (var item in lista)
            {
                ventaViewDeta deta = new ventaViewDeta
                {
                    besafecto_vedeta = item.besafecto_orpedeta,
                    bisc_vedeta = item.ARTICULO.bisc_arti,
                    scod2 = item.ARTICULO.scode_arti,
                    sdesc = item.ARTICULO.sdesc1_arti,
                    nafecto_vedeta = item.nafecto_orpedeta,
                    ncant_vedeta = item.ncantventa_orpedeta,
                    ncode_arti = item.ncode_arti,
                    npu_vedeta = item.npu_orpedeta,
                    ndscto_vedeta = item.ndscto_orpedeta,
                    nexon_vedeta = item.nexon_orpedeta
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
                ventaViewDetas = listadeta
            };

            return Json(ventaView);
        }

        public JsonResult getOrdenCompra(Int32 ncode_orco)
        {
            db.Configuration.ProxyCreationEnabled = false;
            ORDEN_COMPRAS pedido = db.ORDEN_COMPRAS.Find(ncode_orco);
            PROVEEDOR pROVEEDOR = db.PROVEEDOR.Find(pedido.ncode_provee);
            List<ORDEN_COMPRAS_DETALLE> lista = db.ORDEN_COMPRAS_DETALLE.Include("ARTICULO")
                .Where(p => p.ncode_orco == ncode_orco).Where(p => p.ncantventa_orcodeta > 0).ToList();
            List<compraViewDeta> listadeta = new List<compraViewDeta>();

            foreach (var item in lista)
            {
                compraViewDeta deta = new compraViewDeta
                {

                    besafecto_comdeta = item.besafecto_orcodeta,
                    bisc_comdeta = false,
                    scod2 = item.ARTICULO.scode_arti,
                    sdesc = item.ARTICULO.sdesc1_arti,
                    //sund = item.ARTICULO.UMEDIDA.ssunat_umed,
                    nafecto_comdeta = item.nafecto_orcodeta,
                    ncant_comdeta = item.ncantventa_orcodeta,
                    ncode_arti = item.ncode_arti,
                    npu_comdeta = item.npu_orcodeta,
                    ndscto_comdeta = item.ndscto_orcodeta,
                    nexon_comdeta = item.nexon_orcodeta
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
                compraViewDetas = listadeta
            };

            return Json(_compraView);
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