using MarketASP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    [Authorize]
    public class VENDEDORController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0162", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(db.Pr_VendedoresLista(0).ToList());
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0163", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VENDEDOR vENDEDOR)
        {
            if (ModelState.IsValid)
            {
                vENDEDOR.nesta_vende = true;
                vENDEDOR.suser_vende = User.Identity.Name;
                vENDEDOR.dfech_vende = DateTime.Now;
                db.VENDEDOR.Add(vENDEDOR);
                await db.SaveChangesAsync();

                //agregar vendedor zona
                VENDEDOR_ZONA vENDEDOR_ZONA = new VENDEDOR_ZONA();
                vENDEDOR_ZONA.ncode_vende = vENDEDOR.ncode_vende;
                vENDEDOR_ZONA.ncode_zona = vENDEDOR.ncode_zona;
                db.VENDEDOR_ZONA.Add(vENDEDOR_ZONA);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona");
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma");
            return View(vENDEDOR);
        }

        public async Task<ActionResult> Edit(int? id,int? idzona)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0164", xcode);
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
            VENDEDOR vENDEDOR = await db.VENDEDOR.FindAsync(id);
            if (vENDEDOR == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona",idzona);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma",vENDEDOR.ncode_alma);
            return View(vENDEDOR);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VENDEDOR vENDEDOR)
        {
            if (ModelState.IsValid)
            {
                vENDEDOR.susmo_vende = User.Identity.Name;
                vENDEDOR.dfemo_vende = DateTime.Now;

                db.Entry(vENDEDOR).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona", vENDEDOR.ncode_zona);
            ViewBag.ncode_alma = new SelectList(db.ALMACEN.Where(c => c.besta_alma == true), "ncode_alma", "sdesc_alma", vENDEDOR.ncode_alma);
            return View(vENDEDOR);
        }

        public async Task<ActionResult> Deletevendedor(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0165", xcode);
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

            VENDEDOR vENDEDOR = await db.VENDEDOR.FindAsync(id);
            db.VENDEDOR.Remove(vENDEDOR);
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