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
using MarketASP.Areas.Inventario.Clases;

namespace MarketASP.Areas.Inventario.Controllers
{
    public class MOVIMIENTOesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: MOVIMIENTOes
        public async Task<ActionResult> Index()
        {
            var mOVIMIENTO = db.MOVIMIENTO.Include(m => m.ALMACEN).Include(m => m.ALMACEN1).Include(m => m.TIPO_MOVIMIENTO);
            return View(await mOVIMIENTO.ToListAsync());
        }

        // GET: MOVIMIENTOes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            if (mOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(mOVIMIENTO);
        }

        // GET: MOVIMIENTOes/Create
        public ActionResult Create()
        {
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma==true), "ncode_alma", "sdesc_alma");
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN.Where(a => a.besta_alma == true), "ncode_alma", "sdesc_alma");
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO.Where(a => a.besta_tipomovi == true), "ncode_timovi", "sdesc_timovi");
            ViewBag.smone_movi = new SelectList(db.CONFIGURACION.Where(c=>c.besta_confi== true ).Where(c=>c.ntipo_confi==2), "svalor_confi", "sdesc_confi");
            var xfecha = db.TIPO_CAMBIO.Max(v => v.dfecha_tc);
            var yfecha = DateTime.Now.Date;
            var result = db.TIPO_CAMBIO.SingleOrDefault(x => x.dfecha_tc ==  yfecha);
            if (result == null)
            {
                return RedirectToAction("Create","Tipo_Cambio",new {area=""});
            }
            ViewBag.tc = result.nventa_tc;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(string model_json)
        {
            ObjectParameter sw = new ObjectParameter("sw", typeof(int));

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
                        var mofView = JsonConvert.DeserializeObject<moviView>(data, jsonSettings);

                        if (mofView != null)
                        {

                            db.Pr_movimientoCrear(DateTime.Parse(mofView.dfemov_movi), mofView.smone_movi, mofView.ntc_movi, mofView.sobse_movi,
                                "", "", User.Identity.Name, mofView.ncode_timovi, mofView.ncode_alma, mofView.ndestino_alma, "", sw);

                            code = int.Parse(sw.Value.ToString());

                            if (mofView.moviViewDetas != null)
                            {
                                foreach (moviViewDeta item in mofView.moviViewDetas)
                                {
                                    fila++;
                                    db.Pr_movimientoDetaCrea(item.ncode_arti, item.ncant_movidet, item.npu_movidet, User.Identity.Name, code);
                                };

                            }
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


        // GET: MOVIMIENTOes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            if (mOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ndestino_alma);
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO, "ncode_timovi", "sdesc_timovi", mOVIMIENTO.ncode_timovi);
            return View(mOVIMIENTO);
        }

        // POST: MOVIMIENTOes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_movi,dfemov_movi,smone_movi,ntc_movi,sobse_movi,besta_movi,sserie_movi,snume_movi,suser_movi,dfech_movi,susmo_movi,dfemo_movi,ncode_timovi,ncode_alma,ndestino_alma,stipo_movi")] MOVIMIENTO mOVIMIENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mOVIMIENTO).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ncode_alma);
            ViewBag.ndestino_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", mOVIMIENTO.ndestino_alma);
            ViewBag.ncode_timovi = new SelectList(db.TIPO_MOVIMIENTO, "ncode_timovi", "sdesc_timovi", mOVIMIENTO.ncode_timovi);
            return View(mOVIMIENTO);
        }

        // GET: MOVIMIENTOes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            if (mOVIMIENTO == null)
            {
                return HttpNotFound();
            }
            return View(mOVIMIENTO);
        }

        // POST: MOVIMIENTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MOVIMIENTO mOVIMIENTO = await db.MOVIMIENTO.FindAsync(id);
            db.MOVIMIENTO.Remove(mOVIMIENTO);
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
