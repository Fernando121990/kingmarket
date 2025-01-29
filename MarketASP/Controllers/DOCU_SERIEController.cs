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

namespace MarketASP.Controllers
{
    [Authorize]
    public class DOCU_SERIEController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: Administracion/DOCU_SERIE
        public ActionResult Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1005", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var resultado = db.Pr_DocuSerieLista(0).ToList();
            return View(resultado);
        }


        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1006", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L=>L.bacti_local==true), "ncode_local", "sdesc_local");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D=>D.ntipo_confi==5), "ncode_confi", "sdesc_confi");
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DOCU_SERIE dOCU_SERIE,string[] usuarios)
        {
            if (ModelState.IsValid)
            {
                dOCU_SERIE.suser_dose = User.Identity.Name;
                dOCU_SERIE.dfech_dose = DateTime.Now;
                db.DOCU_SERIE.Add(dOCU_SERIE);
                await db.SaveChangesAsync();
                var id = dOCU_SERIE.ncode_dose;


                foreach (var item in usuarios)
                {
                    DOCU_SERIE_USUARIO _docserus = new DOCU_SERIE_USUARIO
                    {
                        ncode_dose = id,
                        susuario_dose = item,
                        suser_dose = User.Identity.Name,
                        dfech_dose = DateTime.Now,
                    };
                    db.DOCU_SERIE_USUARIO.Add(_docserus);
                    await db.SaveChangesAsync();

                }

                return RedirectToAction("Index");
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L => L.bacti_local == true), "ncode_local", "sdesc_local");
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D => D.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName");
            return View(dOCU_SERIE);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1007", xcode);
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
            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }

            serieView serie = new serieView
            {
                ncode_local = dOCU_SERIE.ncode_local,
                besta_dose = dOCU_SERIE.besta_dose,
                ncode_docu = dOCU_SERIE.ncode_docu,
                sserie_dose = dOCU_SERIE.sserie_dose,
                snumeracion_dose = dOCU_SERIE.snumeracion_dose,
                ncode_dose = dOCU_SERIE.ncode_dose
            };


            string[] _usuariosSel = (from s in db.DOCU_SERIE_USUARIO
                                     where s.ncode_dose == id
                                     select s.susuario_dose).ToArray();


            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L => L.bacti_local == true), "ncode_local", "sdesc_local",dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D => D.ntipo_confi == 5), "ncode_confi", "sdesc_confi",dOCU_SERIE.ncode_docu);
            //ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName",result);
            ViewBag.susuario_dose = new MultiSelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName", _usuariosSel);

            return View(serie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(DOCU_SERIE dOCU_SERIE, string[] usuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dOCU_SERIE).State = EntityState.Modified;
                await db.SaveChangesAsync();

                db.Pr_DocuSerieEliminarUsuarios(dOCU_SERIE.ncode_dose);

                foreach (var item in usuarios)
                {
                    db.Pr_DocuSerieCrearUsuarios(dOCU_SERIE.ncode_dose, item, User.Identity.Name);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ncode_local = new SelectList(db.LOCAL.Where(L => L.bacti_local == true), "ncode_local", "sdesc_local", dOCU_SERIE.ncode_local);
            ViewBag.ncode_docu = new SelectList(db.CONFIGURACION.Where(D => D.ntipo_confi == 5), "ncode_confi", "sdesc_confi", dOCU_SERIE.ncode_docu);
            ViewBag.susuario_dose = new SelectList(db.AspNetUsers.OrderByDescending(L => L.UserName), "UserName", "UserName", dOCU_SERIE.susuario_dose);

            return View(dOCU_SERIE);
        }


        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }
            return View(dOCU_SERIE);
        }



        public async Task<ActionResult> DeleteDocSerie(long? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1008", xcode);
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

            DOCU_SERIE dOCU_SERIE = await db.DOCU_SERIE.FindAsync(id);
            if (dOCU_SERIE == null)
            {
                return HttpNotFound();
            }
            db.DOCU_SERIE.Remove(dOCU_SERIE);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult CreateUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUsuario(DOCU_SERIE_USUARIO dOCU_SERIE_USUARIO)
        {
            if (ModelState.IsValid)
            {
                db.DOCU_SERIE_USUARIO.Add(dOCU_SERIE_USUARIO);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dOCU_SERIE_USUARIO);
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
