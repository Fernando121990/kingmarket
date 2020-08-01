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
    public class CLIENTEsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLIENTEs
        public async Task<ActionResult> Index()
        {
            return View(await db.CLIENTE.ToListAsync());
        }

        // GET: CLIENTEs/Details/5
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

        // GET: CLIENTEs/Create
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
                scode_ubigeo = cLIENTE.subigeo_cliente,
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

        // GET: CLIENTEs/Edit/5
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
            return View(cLIENTE);
        }

        // POST: CLIENTEs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_cliente,srazon_cliente,sruc_cliente,sdnice_cliente,sfono1_cliente,sfax_cliente,slineacred_cliente,srepre_cliente,smail_cliente,sweb_cliente,sobse_cliente,sfono2_cliente,sfono3_cliente,sappa_cliente,sapma_cliente,snomb_cliente,bprocedencia_cliente,suser_cliente,dfech_cliente,susmo_cliente,dfemo_cliente")] CLIENTE cLIENTE)
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
