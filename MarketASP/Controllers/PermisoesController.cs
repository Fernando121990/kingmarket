using MarketASP.Clases;
using MarketASP.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using System;

namespace MarketASP.Controllers
{
    [Authorize]
    public class PermisoesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();


        public ActionResult Create(string username)
        {
            
            ViewBag.username = new SelectList(db.AspNetUsers, "username", "username",username);

            List<ventanaView> ldeta = new List<ventanaView>();
            permisoView _permiso = new permisoView();
                
            var _permisos = db.Pr_PermisoLista(username).ToList();
            foreach (var item in _permisos)
            {
                ventanaView ventana = new ventanaView
                    {   Menu = item.Menu,
                        Nivel = item.nivel,
                        Operacion = item.Operacion,
                        acceso = bool.Parse(item.Acceso)
                    };
                ldeta.Add(ventana);
            }

            _permiso.detalle = ldeta;

            return View(_permiso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(permisoView permiso)
        {
            if (ModelState.IsValid)
            {
                db.Pr_PermisoEliminar(permiso.username);
                foreach (var item in permiso.detalle)
                {
                    if (item.acceso == true)
                    {
                        db.Pr_PermisoCrear(permiso.username, item.Nivel);
                    }
                }

                return RedirectToAction("Create",new {username = permiso.username });
            }

            return View(permiso);
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
