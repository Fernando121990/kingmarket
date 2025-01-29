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
using Newtonsoft.Json;
using MarketASP.Clases;
using MarketASP.Extensiones;
using System.Security;

namespace MarketASP.Controllers
{
    [Authorize]
    public class COMPRASController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: COMPRAS
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0301", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            //var cOMPRAS = db.COMPRAS.Include(c => c.ALMACEN).Include(c => c.PROVEEDOR).Include(c => c.CONFIGURACION).Include(c => c.CONFIGURACION1);
            var resultado = db.Pr_CompraLista("", "", "", "").ToList();
            return View(resultado);
        }

        // GET: COMPRAS/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }
            return View(cOMPRAS);
        }

        // GET: COMPRAS/Create
        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0302", xcode);
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
                return RedirectToAction("Create", "Tipo_Cambio", new { area = "" });
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";

            ViewBag.tc = result.nventa_tc;
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5).Where(c=>c.svalor_confi == "C"), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.smone_compra = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.dfecompra_compra = string.Format("{0:dd/MM/yyyy}", yfecha);
            ViewBag.dfevenci_compra = string.Format("{0:dd/MM/yyyy}", yfecha);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            int code;
            string data = "";
            int fila = 0;
            string xmensaje = "";
            int exito = 0;

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
                        var mofView = JsonConvert.DeserializeObject<compraView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_compraCrea( mofView.sseri_compra, mofView.snume_compra, DateTime.Parse(mofView.sfecompra_compra),
                                DateTime.Parse(mofView.sfevenci_compra), mofView.smone_compra, mofView.ntc_compra, mofView.sobs_compra,
                                mofView.sguia_compra, mofView.sproforma_compra, mofView.nbrutoex_compra, mofView.nbrutoaf_compra,
                                mofView.ndsctoex_compra, mofView.ndsctoaf_compra, mofView.nsubex_compra, mofView.nsubaf_compra, mofView.nigvex_compra,
                                mofView.nigvaf_compra, mofView.ntotaex_compra, mofView.ntotaaf_compra, mofView.ntotal_compra, mofView.ntotalMN_compra,
                                mofView.ntotalUS_compra,mofView.nvalIGV_compra, User.Identity.Name, mofView.ncode_alma,mofView.ncode_provee,
                                mofView.ncode_docu,mofView.ncode_fopago, int.Parse(User.Identity.GetLocal()),mofView.ncode_orco,
                                mofView.sserie_orco,mofView.stipo_orco, sw);


                            code = int.Parse(sw.Value.ToString());

                            if (mofView.compraViewDetas != null)
                            {
                                foreach (compraViewDeta item in mofView.compraViewDetas)
                                {
                                    fila++;
                                    db.Pr_compraDetaCrea(item.ncant_comdeta, item.npu_comdeta,
                                        item.ndscto_comdeta, item.ndscto2_comdeta, item.nexon_comdeta, item.nafecto_comdeta, item.besafecto_comdeta,
                                        code, item.ncode_alma, item.ncode_arti,item.ncantLote_comdeta,fila,item.ncode_orco);
                                };

                            }


                            if (mofView.loteViewDeta != null)
                            {

                                db.Pr_LoteEditar(code);

                                foreach (loteViewDeta item in mofView.loteViewDeta)
                                {
                                    fila++;

                                    DateTime xfecha = DateTime.Parse(item.sfechalote);

                                    db.Pr_LoteCrear(item.sdesc_lote, xfecha, item.ncode_arti, code,
                                        item.ncant_lote, User.Identity.Name,"Compra",6,"I",code,mofView.ncode_alma,
                                        mofView.sseri_compra,mofView.snume_compra,code, mensaje, sw);

                                    xmensaje += mensaje.Value.ToString();

                                    if (mensaje.Value.ToString() == "NO")
                                    {
                                        exito = 0;
                                        break;
                                    }

                                    //db.Pr_LoteActualizarCompra(item.ncode_compra, item.ncode_arti);

                                    exito = 1;
                                };

                                db.Pr_LoteDetaEditar(code);

                                db.Pr_compraActualizaLote(0, 0, code);

                            }

                            if (mofView.stipo_orco != "OS")
                            {
                                db.Pr_KardexCrea("Compra", 6, "I", code, User.Identity.Name);

                                if (mofView.ncode_orco != null && mofView.ncode_orco > 0)
                                {
                                    db.Pr_KardexCrea("Compra", 6, "A", code, User.Identity.Name);
                                }

                            }



                           // db.Pr_compraActualizaPedido(0, mofView.ncode_orco, code);
                        }
                    }
                }

                return Json(new { Success = 1 });

            }
            catch (Exception ex)
            {
                string mensajeX = ex.Message;
                ViewBag.mensaje = mensajeX;
                return Json(new { Success = 0 });
            }
        }

        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0303", xcode);
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
            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.icbper = Helpers.Funciones.ObtenerValorParam("GENERAL", "ICBPER");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");
            ViewBag.precioconigv = Helpers.Funciones.ObtenerValorParam("GENERAL", "PRECIO CON IGV") == "SI" ? "Checked" : "Unchecked";

            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5).Where(c => c.svalor_confi == "C"), "ncode_confi", "sdesc_confi", cOMPRAS.ncode_docu);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", cOMPRAS.ncode_fopago);
            ViewBag.smone_compra = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", cOMPRAS.smone_compra);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", cOMPRAS.ncode_alma);
            ViewBag.sdesc_prove = cOMPRAS.PROVEEDOR.sdesc_prove;
            ViewBag.ncode_provee = cOMPRAS.ncode_provee ;
            ViewBag.tc = cOMPRAS.ntc_compra;
            ViewBag.dfecompra_compra = string.Format("{0:dd/MM/yyyy}", cOMPRAS.dfecompra_compra);
            ViewBag.dfevenci_compra = string.Format("{0:dd/MM/yyyy}", cOMPRAS.dfevenci_compra);
            return View(cOMPRAS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            int xsw;
            long code;
            string data = "";
            int fila = 0;
            string xmensaje = "";
            int exito = 0;


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
                        var mofView = JsonConvert.DeserializeObject<compraView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_compraEdita(mofView.ncode_compra ,mofView.sseri_compra, mofView.snume_compra, DateTime.Parse(mofView.sfecompra_compra),
                                DateTime.Parse(mofView.sfevenci_compra), mofView.smone_compra, mofView.ntc_compra, mofView.sobs_compra,
                                mofView.sguia_compra, mofView.sproforma_compra, mofView.nbrutoex_compra, mofView.nbrutoaf_compra,
                                mofView.ndsctoex_compra, mofView.ndsctoaf_compra, mofView.nsubex_compra, mofView.nsubaf_compra, mofView.nigvex_compra,
                                mofView.nigvaf_compra, mofView.ntotaex_compra, mofView.ntotaaf_compra, mofView.ntotal_compra, mofView.ntotalMN_compra,
                                mofView.ntotalUS_compra, mofView.nvalIGV_compra, User.Identity.Name, mofView.ncode_alma, mofView.ncode_provee,
                                mofView.ncode_docu, mofView.ncode_fopago, int.Parse(User.Identity.GetLocal()), sw);

                            xsw = int.Parse(sw.Value.ToString());
                            code = mofView.ncode_compra;

                            if (mofView.compraViewDetas != null)
                            {
                                foreach (compraViewDeta item in mofView.compraViewDetas)
                                {
                                    fila++;
                                    db.Pr_compraDetaCrea(item.ncant_comdeta, item.npu_comdeta,
                                        item.ndscto_comdeta, item.ndscto2_comdeta, item.nexon_comdeta, item.nafecto_comdeta, item.besafecto_comdeta,
                                        code, item.ncode_alma, item.ncode_arti,item.ncantLote_comdeta,fila,item.ncode_orco);
                                };

                            }


                            if (mofView.loteViewDeta != null)
                            {

                                db.Pr_LoteEditar(code);

                                foreach (loteViewDeta item in mofView.loteViewDeta)
                                {
                                    fila++;

                                    DateTime xfecha = DateTime.Parse(item.sfechalote);

                                    db.Pr_LoteCrear(item.sdesc_lote, xfecha, item.ncode_arti, code,
                                        item.ncant_lote, User.Identity.Name, "Compra", 6, "I", code, mofView.ncode_alma,
                                        mofView.sseri_compra, mofView.snume_compra, code, mensaje, sw);


                                    xmensaje += mensaje.Value.ToString();

                                    if (mensaje.Value.ToString() == "NO")
                                    {
                                        exito = 0;
                                        break;
                                    }

                                    //db.Pr_LoteActualizarCompra(item.ncode_compra, item.ncode_arti);

                                    exito = 1;
                                };

                                db.Pr_LoteDetaEditar(code);

                                db.Pr_compraActualizaLote(0, 0, code);

                            }



                            db.Pr_compraDetaEdita(code);

                            db.Pr_KardexElimina("compra", code);

                            db.Pr_KardexCrea("Compra", 6, "I", code, User.Identity.Name);

                            
                        }
                    }
                }

                return Json(new { Success = 1 });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0 });
            }
        }

        public async Task<ActionResult> anulaCompra(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0306", xcode);
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

            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_compraAnula(id, sw);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteCompra(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0304", xcode);
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

            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }

            db.Pr_compraElimina(id, sw);
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> CreateLote(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0305", xcode);
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
            COMPRAS cOMPRAS = await db.COMPRAS.FindAsync(id);
            if (cOMPRAS == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma",cOMPRAS.ncode_alma);
            return View(cOMPRAS);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateLote(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            string data = "";
            string xmensaje = "";
            int fila = 0;
            int exito = 0;
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
                        var mofView = JsonConvert.DeserializeObject<loteView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_LoteEditar(mofView.ncode_compra);

                            if (mofView.loteViewDeta != null)
                            {

                                foreach (loteViewDeta item in mofView.loteViewDeta)
                                {
                                    fila++;

                                    DateTime xfecha = DateTime.Parse(item.sfechalote);

                                    db.Pr_LoteCrear(item.sdesc_lote, xfecha, item.ncode_arti, 0,
                                        item.ncant_lote, User.Identity.Name, "Compra", 6, "I", 0, 0,
                                        "", "", 0, mensaje, sw);

                                    xmensaje += mensaje.Value.ToString();

                                    if (mensaje.Value.ToString() == "NO")
                                    {
                                        exito = 0;
                                        break;
                                    }

                                    //db.Pr_LoteActualizarCompra(item.ncode_compra, item.ncode_arti);
                                    
                                    exito = 1;
                                };

                            }

                            db.Pr_LoteDetaEditar(mofView.ncode_compra);

                            db.Pr_compraActualizaLote(0,0, mofView.ncode_compra);
                        }
                    }
                }

                return Json(new { Success = exito });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0 });
            }
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
