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
    public class RecetasController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Recetas
        public async Task<ActionResult> Index()
        {
            return View(await db.Receta.ToListAsync());
        }

        // GET: Recetas/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receta receta = await db.Receta.FindAsync(id);
            if (receta == null)
            {
                return HttpNotFound();
            }
            return View(receta);
        }

        // GET: Recetas/Create
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
            ViewBag.Rec_almacen = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.Rec_codProd = new SelectList(db.ARTICULO.Where(c => c.nesta_arti == true).OrderByDescending(c =>c.sdesc1_arti), "ncode_arti", "sdesc1_arti");
            ViewBag.Rec_tipo = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 13), "svalor_confi", "sdesc_confi");
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
                        var mofView = JsonConvert.DeserializeObject<recetaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_RecetaCrear("N",0,mofView.Rec_descripcion,mofView.Rec_codclase,mofView.Rec_codProd,
                                mofView.Rec_cantidad,mofView.Rec_almacen,mofView.Rec_tipo,mofView.Rec_costoOperativo,
                                int.Parse(mofView.Rec_almacen),ID,ingproc_001);

                            code = int.Parse(ID.Value.ToString());

                            if (mofView.recetaViewDetas != null)
                            {
                                foreach (recetaViewDeta item in mofView.recetaViewDetas)
                                {
                                    fila++;
                                    db.Pr_RecetaDetalle(code,"",item.RecD_CodProd,item.RecD_Cantidad,mofView.Rec_almacen,item.RecD_CodProdPadre,
                                        item.RecD_tipo,item.RecD_coidgoPadre,item.RecD_precio);
                                };

                            }

                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Receta Registrada" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0, Mensaje = mensajex });
            }

        }

        // GET: Recetas/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receta receta = await db.Receta.FindAsync(id);
            if (receta == null)
            {
                return HttpNotFound();
            }
            ViewBag.igv = Helpers.Funciones.ObtenerValorParam("GENERAL", "IGV");
            ViewBag.deci = Helpers.Funciones.ObtenerValorParam("GENERAL", "No DE DECIMALES");
            ViewBag.moneda = Helpers.Funciones.ObtenerValorParam("GENERAL", "MONEDA X DEFECTO");

            ViewBag.smone_orpe = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 2), "svalor_confi", "sdesc_confi", ViewBag.moneda);
            ViewBag.Rec_almacen = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma",receta.Rec_almacen);
            ViewBag.Rec_codProd = new SelectList(db.ARTICULO.Where(c => c.nesta_arti == true).OrderByDescending(c => c.sdesc1_arti), "ncode_arti", "sdesc1_arti",receta.Rec_codProd);
            ViewBag.Rec_tipo = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 13), "svalor_confi", "sdesc_confi",receta.Rec_tipo);

            return View(receta);
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
                        var mofView = JsonConvert.DeserializeObject<recetaView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_RecetaCrear("M", mofView.Rec_codigo, mofView.Rec_descripcion, mofView.Rec_codclase, mofView.Rec_codProd,
                                mofView.Rec_cantidad, mofView.Rec_almacen, mofView.Rec_tipo, mofView.Rec_costoOperativo,
                                int.Parse(mofView.Rec_almacen), ID, ingproc_001);

                            code = int.Parse(ID.Value.ToString());

                            if (mofView.recetaViewDetas != null)
                            {
                                foreach (recetaViewDeta item in mofView.recetaViewDetas)
                                {
                                    fila++;
                                    db.Pr_RecetaDetalle(code, "", item.RecD_CodProd, item.RecD_Cantidad, mofView.Rec_almacen, item.RecD_CodProdPadre,
                                        item.RecD_tipo, item.RecD_coidgoPadre, item.RecD_precio);
                                };

                            }

                        }
                    }
                }

                return Json(new { Success = 1, Mensaje = "Receta Registrada" });

            }
            catch (Exception ex)
            {
                string mensajex = ex.Message;
                ViewBag.mensaje = mensajex;
                return Json(new { Success = 0, Mensaje = mensajex });
            }
        }

        // GET: Recetas/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receta receta = await db.Receta.FindAsync(id);
            if (receta == null)
            {
                return HttpNotFound();
            }
            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Receta receta = await db.Receta.FindAsync(id);
            db.Receta.Remove(receta);
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
