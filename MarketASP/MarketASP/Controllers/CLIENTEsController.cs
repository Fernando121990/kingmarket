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
using MarketASP.Clases;
using System.Data.Entity.Core.Objects;

namespace MarketASP.Controllers
{
    [Authorize]
    public class CLIENTEsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLIENTEs
        public async Task<ActionResult> Index()
        {
            return View(await db.CLIENTE.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = await db.CLIENTE.FindAsync(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(clienteView cLIENTE)
        {
            if (ModelState.IsValid)
            {
                //VERIFICAR RUC O DNI

                if (cLIENTE.stipo_cliente == "J")
                {
                    CLIENTE xcli = await db.CLIENTE.SingleOrDefaultAsync(c => c.sruc_cliente == cLIENTE.sruc_cliente);
                    if (xcli != null)
                    {
                        ViewBag.mensaje = "El cliente ya existe";
                        return View("_Mensaje");
                    }

                }

                if (cLIENTE.stipo_cliente == "N")
                {
                    CLIENTE xcli = await db.CLIENTE.SingleOrDefaultAsync(c => c.sdnice_cliente == cLIENTE.sdnice_cliente);
                    if (xcli != null)
                    {
                        ViewBag.mensaje = "El cliente ya existe";
                        return View("_Mensaje");
                    }

                }

                CLIENTE cliente =  ToCliente(cLIENTE);

                db.CLIENTE.Add(cliente);
                await db.SaveChangesAsync();

                int id = cliente.ncode_cliente;
                CLI_DIRE cLI_DIRE = toCliDire(cLIENTE,id);

                db.CLI_DIRE.Add(cLI_DIRE);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(cLIENTE);
        }

        private CLI_DIRE toCliDire(clienteView cLIENTE,int id)
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
                srazon_cliente = cliView.srazon_cliente,
                srepre_cliente = cliView.srepre_cliente,
                sruc_cliente = cliView.sruc_cliente,
                stipo_cliente = cliView.stipo_cliente,
                suser_cliente = User.Identity.Name,
                sweb_cliente = cliView.sweb_cliente,
            };

        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = await db.CLIENTE.FindAsync(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }

            if (cLIENTE.stipo_cliente == "J")
            {
                ViewBag.pju = "Checked";
                ViewBag.pna = "";
            }
            else
            {
                ViewBag.pju = "";
                ViewBag.pna = "Checked";
            }

            return View(cLIENTE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CLIENTE cLIENTE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLIENTE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cLIENTE);
        }
        public async Task<ActionResult> DeleteCliente(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CLIENTE cLIENTE = await db.CLIENTE.FindAsync(id);

            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            db.Pr_ClienteElimina(id, sw);

            int xsw = int.Parse(sw.Value.ToString());

            if (xsw == 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(cLIENTE);
            }
           
        }

        public ActionResult CreateDireccion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ncode_cliente = id; //new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente");
            ViewBag.scode_ubigeo = new SelectList(db.UBIGEO, "scode_ubigeo", "sdepa_ubigeo");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDireccion(CLI_DIRE cLI_DIRE)
        {
            if (ModelState.IsValid)
            {
                db.CLI_DIRE.Add(cLI_DIRE);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","Clientes",new { id = cLI_DIRE.ncode_cliente});
            }

            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cLI_DIRE.ncode_cliente);
            ViewBag.scode_ubigeo = new SelectList(db.UBIGEO, "scode_ubigeo", "sdepa_ubigeo", cLI_DIRE.scode_ubigeo);
            return View(cLI_DIRE);
        }

        public async Task<ActionResult> DeleteDire(int? id)
        {
            Int64 ncodeCliente = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CLI_DIRE cLI_DIRE  = await db.CLI_DIRE.FindAsync(id);
            ncodeCliente = cLI_DIRE.ncode_cliente;

            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            db.Pr_clienteDireElimina(id, sw);

            int xsw = int.Parse(sw.Value.ToString());

            if (xsw == 0)
            {
                return RedirectToAction("Details", "Clientes", new { id = ncodeCliente });
            }
            else
            {
                ViewBag.mensaje = "No se puede eliminar registro";
                return View("_Mensaje");
            }
        }

        public async Task<ActionResult> EditDire(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLI_DIRE cLI_DIRE = await db.CLI_DIRE.FindAsync(id);
            if (cLI_DIRE == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_cliente = cLI_DIRE.ncode_cliente;
            ViewBag.scode_ubigeo = cLI_DIRE.scode_ubigeo;
            ViewBag.subigeo = cLI_DIRE.UBIGEO.sdistri_ubigeo;
            return View(cLI_DIRE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDire(CLI_DIRE cLI_DIRE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLI_DIRE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Clientes", new { id = cLI_DIRE.ncode_cliente });
            }
            ViewBag.ncode_cliente = cLI_DIRE.ncode_cliente;
            ViewBag.scode_ubigeo = cLI_DIRE.scode_ubigeo;
            ViewBag.subigeo = cLI_DIRE.UBIGEO.sdistri_ubigeo;
            return View(cLI_DIRE);
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
