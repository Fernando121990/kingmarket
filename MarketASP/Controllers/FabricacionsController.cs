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

namespace MarketASP.Controllers
{
    public class FabricacionsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Fabricacions
        public async Task<ActionResult> Index()
        {
            return View(await db.Fabricacion.ToListAsync());
        }

        // GET: Fabricacions/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            if (fabricacion == null)
            {
                return HttpNotFound();
            }
            return View(fabricacion);
        }

        // GET: Fabricacions/Create
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

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");

            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ViewBag.moneda);
            ViewBag.Fab_almacen = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.Rec_codigo = new SelectList(db.Receta.OrderByDescending(c => c.Rec_descripcion), "Rec_codigo", "Rec_descripcion");
            ViewBag.Fab_tipo = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 13), "svalor_confi", "sdesc_confi");
            ViewBag.Fab_TipoProd = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 13), "svalor_confi", "sdesc_confi");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter ID = new ObjectParameter("ID", typeof(int));
            ObjectParameter ingproc_001 = new ObjectParameter("ingproc_001", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            int code;
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
                        var mofView = JsonConvert.DeserializeObject<fabricacionView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_Fabricacion("N",mofView.Rec_Codigo,0, mofView.Fab_Tipo, mofView.Fab_NroDoc,
                                DateTime.Parse(mofView.sfab_fecha), mofView.Fab_TipoCambio, mofView.Fab_Glosa, mofView.Fab_Lote,
                                mofView.Fab_Estado,mofView.Fab_TipoProd,DateTime.Parse(mofView.sfab_fvenc),mofView.Fab_Cantidad,
                                "","",mofView.Fab_CostoUnit,mofView.Fab_CostoTotalMN,mofView.Fab_CostoTotalUS,
                                mofView.Fab_CostoOperativo,mofView.Fab_almacen,ID,ingproc_001);

                            code = int.Parse(ID.Value.ToString());

                            if (mofView.fabricacionViewDetas != null)
                            {
                                foreach (fabricacionViewDeta item in mofView.fabricacionViewDetas)
                                {
                                    fila++;
                                    db.Pr_FabricacionDetalleCrea(item.FabD_tipo,item.FabD_NroDoc,item.FabD_CodClase,
                                        item.FabD_CodProd, item.FabD_Cantidad, item.FabD_Costo_D,item.FabD_Costo_S,
                                        item.FabD_Almacen, item.FabD_Mov,item.FabD_TipoDetalle,item.FabD_CodClase_Ref,
                                        item.FabD_CodProd_Ref,item.FabD_CodProd_Ini,item.FabD_CodClase_Ini,item.FabD_CantUtil,
                                        item.FabD_Adicional,code,item.RecD_Cantidad);
                                };

                            }

                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Fabricacion Registrada" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0, Mensaje = mensajex });
            }

        }

        // GET: Fabricacions/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.Pr_PermisoAcceso(User.Identity.Name, "0802", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");

            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ViewBag.moneda);
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            if (fabricacion == null)
            {
                return HttpNotFound();
            }

            ViewBag.Fab_almacen = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma",fabricacion.Fab_almacen);
            ViewBag.Rec_codigo = new SelectList(db.Receta.OrderByDescending(c => c.Rec_descripcion), "Rec_codigo", "Rec_descripcion",fabricacion.Rec_Codigo);
            ViewBag.Fab_tipo = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 13), "svalor_confi", "sdesc_confi",fabricacion.Fab_Tipo);
            ViewBag.Fab_TipoProd = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 13), "svalor_confi", "sdesc_confi",fabricacion.Fab_TipoProd);

            return View(fabricacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(string model_json)
        {
            ObjectParameter ID = new ObjectParameter("ID", typeof(int));
            ObjectParameter ingproc_001 = new ObjectParameter("ingproc_001", typeof(int));
            ObjectParameter mensaje = new ObjectParameter("mensaje", typeof(string));

            int code;
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
                        var mofView = JsonConvert.DeserializeObject<fabricacionView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_Fabricacion("M", mofView.Rec_Codigo,mofView.Fab_Codigo, mofView.Fab_Tipo, mofView.Fab_NroDoc,
                                DateTime.Parse(mofView.sfab_fecha), mofView.Fab_TipoCambio, mofView.Fab_Glosa, mofView.Fab_Lote,
                                mofView.Fab_Estado, mofView.Fab_TipoProd, DateTime.Parse(mofView.sfab_fvenc), mofView.Fab_Cantidad,
                                "", "", mofView.Fab_CostoUnit, mofView.Fab_CostoTotalMN, mofView.Fab_CostoTotalUS,
                                mofView.Fab_CostoOperativo, mofView.Fab_almacen, ID, ingproc_001);

                            code = int.Parse(ID.Value.ToString());

                            if (mofView.fabricacionViewDetas != null)
                            {
                                foreach (fabricacionViewDeta item in mofView.fabricacionViewDetas)
                                {
                                    fila++;
                                    db.Pr_FabricacionDetalleCrea(item.FabD_tipo, item.FabD_NroDoc, item.FabD_CodClase,
                                        item.FabD_CodProd, item.FabD_Cantidad, item.FabD_Costo_D, item.FabD_Costo_S,
                                        item.FabD_Almacen, item.FabD_Mov, item.FabD_TipoDetalle, item.FabD_CodClase_Ref,
                                        item.FabD_CodProd_Ref, item.FabD_CodProd_Ini, item.FabD_CodClase_Ini, item.FabD_CantUtil,
                                        item.FabD_Adicional, code, item.RecD_Cantidad);
                                };

                            }

                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Fabricacion Registrada" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0, Mensaje = mensajex });
            }

        }

        // GET: Fabricacions/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            if (fabricacion == null)
            {
                return HttpNotFound();
            }
            return View(fabricacion);
        }

        // POST: Fabricacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Fabricacion fabricacion = await db.Fabricacion.FindAsync(id);
            db.Fabricacion.Remove(fabricacion);
            await db.SaveChangesAsync();
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
