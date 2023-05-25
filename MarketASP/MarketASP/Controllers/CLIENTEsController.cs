using MarketASP.Clases;
using MarketASP.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    [Authorize]
    public class CLIENTEsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLIENTEs
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1201", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

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
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1202", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(clienteView cLIENTE)
        {
            if (ModelState.IsValid)
            {
                try
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
                catch (Exception ex)
                {
                    ViewBag.mensaje = ex.Message.ToString();
                    return View("_mensaje");
                }

            }

            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona");
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
                srazon_cliente =  (cliView.stipo_cliente == "J") ? cliView.srazon_cliente : cliView.snomb_cliente + " " + cliView.sappa_cliente + " " + cliView.sapma_cliente ,
                srepre_cliente = cliView.srepre_cliente,
                sruc_cliente = cliView.sruc_cliente,
                stipo_cliente = cliView.stipo_cliente,
                suser_cliente = User.Identity.Name,
                sweb_cliente = cliView.sweb_cliente,
                bacti_cliente = true,
                ncode_fopago = cliView.ncode_fopago,
                ncode_afepercepcion = cliView.ncode_afepercepcion
            };

        }

        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1203", xcode);
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

            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi",cLIENTE.ncode_fopago);
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi",cLIENTE.ncode_afepercepcion);
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona",cLIENTE.ncode_zona);
            return View(cLIENTE);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CLIENTE cLIENTE)
        {
            if (ModelState.IsValid)
            {
                cLIENTE.srazon_cliente = (cLIENTE.stipo_cliente == "J") ? cLIENTE.srazon_cliente : cLIENTE.snomb_cliente + " " + cLIENTE.sappa_cliente + " " + cLIENTE.sapma_cliente;
                db.Entry(cLIENTE).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 6), "ncode_confi", "sdesc_confi", cLIENTE.ncode_fopago);
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi", cLIENTE.ncode_afepercepcion);
            ViewBag.ncode_zona = new SelectList(db.ZONA.Where(c => c.nesta_zona == true), "ncode_zona", "sdesc_zona", cLIENTE.ncode_zona);
            return View(cLIENTE);
        }
        public async Task<ActionResult> DeleteCliente(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "1204", xcode);
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
                ViewBag.mensaje = "No se puede eliminar cliente, tiene registros asociados";
                return View("_Mensaje");
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
