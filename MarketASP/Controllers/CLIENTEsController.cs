using MarketASP.Clases;
using MarketASP.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarketASP.Controllers
{
    [Authorize]
    public class CLIENTEsController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: CLIENTEs
        public ActionResult Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0142", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            return View(db.Pr_ClienteListado(0).ToList());
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
            ViewBag.encargado = "";
            ViewBag.zona = "";

            var xcode = cLIENTE.ncode_vende != null ? cLIENTE.ncode_vende : 0;
            var resultado = db.Pr_VendedoresLista(xcode).ToList();
            if (resultado.Count > 0)
            {
                var encargadoarray = resultado.ToArray();
                ViewBag.encargado = encargadoarray[0].sdesc_vende;
                ViewBag.zona = encargadoarray[0].sdesc_zona;

            }
            return View(cLIENTE);
        }

        public ActionResult Create()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0143", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            ViewBag.nidtipodoc_cliente = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_venzo = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_venzo", "VendeZona");
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

            ViewBag.nidtipodoc_cliente = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi");
            ViewBag.ncode_venzo = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_venzo", "VendeZona");
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
                ncode_afepercepcion = cliView.ncode_afepercepcion,
                ncode_venzo = cliView.ncode_venzo
            };

        }

        public async Task<ActionResult> Edit(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0144", xcode);
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

            ViewBag.nidtipodoc_cliente = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi",cLIENTE.nidtipodoc_cliente);
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi",cLIENTE.ncode_afepercepcion);
            ViewBag.ncode_venzo = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_venzo", "VendeZona", cLIENTE.ncode_venzo);
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
            ViewBag.nidtipodoc_cliente = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 5), "ncode_confi", "sdesc_confi", cLIENTE.nidtipodoc_cliente);
            ViewBag.ncode_afepercepcion = new SelectList(db.CONFIGURACION.Where(c => c.besta_confi == true).Where(c => c.ntipo_confi == 11), "ncode_confi", "sdesc_confi", cLIENTE.ncode_afepercepcion);
            ViewBag.ncode_venzo = new SelectList(db.Pr_VendedorZonaLista(0), "ncode_venzo", "VendeZona", cLIENTE.ncode_venzo);
            return View(cLIENTE);
        }
        public async Task<ActionResult> DeleteCliente(int? id)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0145", xcode);
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
        #region CLIDIRECCION

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
        #endregion
        #region CLIFORMAPAGO

        public ActionResult CreateFopago(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ncode_cliente = id; //new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente");
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(x=> x.ntipo_confi == 6 ), "ncode_confi", "sdesc_confi");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFopago(CLI_FOPAGO cli_fopago)
        {
            if (ModelState.IsValid)
            {
                db.CLI_FOPAGO.Add(cli_fopago);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Clientes", new { id = cli_fopago.ncode_cliente });
            }

            ViewBag.ncode_cliente = new SelectList(db.CLIENTE, "ncode_cliente", "srazon_cliente", cli_fopago.ncode_cliente);
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(x => x.ntipo_confi == 6), "ncode_confi", "sdesc_confi");
            return View(cli_fopago);
        }

        public async Task<ActionResult> DeleteFopago(int? id)
        {
            int ncodeCliente = 0;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CLI_FOPAGO cLI_FOPAGO = await db.CLI_FOPAGO.FindAsync(id);
            ncodeCliente = (int) cLI_FOPAGO.ncode_cliente;

            ObjectParameter sw = new ObjectParameter("sw", typeof(int));
            db.Pr_clientefopagoElimina(id, sw);

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

        public async Task<ActionResult> Editfopago(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLI_FOPAGO cli_fopago = await db.CLI_FOPAGO.FindAsync(id);
            if (cli_fopago == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_cliente = cli_fopago.ncode_cliente;
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(x => x.ntipo_confi == 6), "ncode_confi", "sdesc_confi",cli_fopago.ncode_fopago);
            return View(cli_fopago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Editfopago(CLI_FOPAGO cli_fopago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cli_fopago).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Clientes", new { id = cli_fopago.ncode_cliente });
            }
            ViewBag.ncode_cliente = cli_fopago.ncode_cliente;
            ViewBag.ncode_fopago = new SelectList(db.CONFIGURACION.Where(x => x.ntipo_confi == 6), "ncode_confi", "sdesc_confi", cli_fopago.ncode_fopago);
            return View(cli_fopago);
        }


        #endregion
        [HttpPost]
        public JsonResult BuscarCliente(string tipo, string numero)
        {
            if (tipo == "RUC")
            {
                var cliente = db.CLIENTE.FirstOrDefault(c => c.sruc_cliente == numero);
                if (cliente != null)
                {
                    // Si el cliente ya está registrado, devolver solo el mensaje
                    return Json(new
                    {
                        success = true,
                        mensaje = "El cliente ya está registrado en la base de datos."
                    }, JsonRequestBehavior.AllowGet);
                }
                /*if (cliente != null)
                {
                    return Json(new
                    {
                        success = true,
                        cliente = new
                        {
                            cliente.srazon_cliente,
                            cliente.sfono1_cliente
                        }
                    }, JsonRequestBehavior.AllowGet);
                }*/
                else
                {
                    var hola = "414 hhdjsh";
                    // Buscar en el API por RUC
                    var apiResponse = ConsultarApiRuc(numero);
                    if (apiResponse != null)
                    {
                        // Loguear la respuesta de la API para ver qué datos se están recibiendo
                        Console.WriteLine($"Respuesta de la API RUC: {apiResponse}");

                        // Verificar que la respuesta contiene los datos esperados
                        if (apiResponse.ContainsKey("razonSocial"))
                        {
                            return Json(new
                            {
                                success = true,
                                cliente = new
                                {
                                    srazon_cliente = apiResponse["razonSocial"].ToString(), // Razón social
                                    sdire_cliente= apiResponse["direccion"].ToString(),
                                    scode_ubigeo = apiResponse["ubigeo"].ToString(),
                                    subigeo = $"{apiResponse["distrito"]} {apiResponse["provincia"]} {apiResponse["departamento"]}",
                                    sfono1_cliente = "" // No hay teléfono disponible en la API
                                }
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "No se encontraron datos completos en la API para el RUC." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al consultar la API para el RUC." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else if (tipo == "DNI")
            {
                var cliente = db.CLIENTE.FirstOrDefault(c => c.sdnice_cliente == numero);
                if (cliente != null)
                {
                    // Si el cliente ya está registrado, devolver solo el mensaje
                    return Json(new
                    {
                        success = true,
                        mensaje = "El cliente ya está registrado en la base de datos."
                    }, JsonRequestBehavior.AllowGet);
                }
                /*if (cliente != null)
                {
                    return Json(new
                    {
                        success = true,
                        cliente = new
                        {
                            cliente.snomb_cliente,
                            cliente.sappa_cliente,
                            cliente.sapma_cliente,
                            cliente.sfono1_cliente
                        }
                    }, JsonRequestBehavior.AllowGet);
                }*/
                else
                {
                    // Buscar en el API por DNI
                    var apiResponse = ConsultarApiDni(numero);
                    if (apiResponse != null)
                    {
                        // Loguear la respuesta de la API para ver qué datos se están recibiendo
                        Console.WriteLine($"Respuesta de la API: {apiResponse}");

                        // Verificar que la respuesta contiene los datos esperados
                        if (apiResponse.ContainsKey("nombres") &&
                            apiResponse.ContainsKey("apellidoPaterno") &&
                            apiResponse.ContainsKey("apellidoMaterno"))
                        {
                            return Json(new
                            {
                                success = true,
                                cliente = new
                                {
                                    snomb_cliente = apiResponse["nombres"].ToString(), // Nombres del cliente
                                    sappa_cliente = apiResponse["apellidoPaterno"].ToString(), // Apellido Paterno
                                    sapma_cliente = apiResponse["apellidoMaterno"].ToString(), // Apellido Materno
                                    sfono1_cliente = "" // No hay teléfono disponible en la API
                                }
                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { success = false, message = "No se encontraron datos completos en la API para el DNI." }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al consultar la API para el DNI." }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(new { success = false, message = "Cliente no encontrado." }, JsonRequestBehavior.AllowGet);
        }

        private JObject ConsultarApiRuc(string numero)
        {
            try
            {
                string token = "apis-token-12635.jyQnRC3L3MHYAGvpHi8zHj3qRKJKYmkB";
                string apiUrl = $"https://api.apis.net.pe/v2/sunat/ruc?numero={numero}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = client.GetAsync(apiUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        // Loguear la respuesta para ver qué se está recibiendo
                        Console.WriteLine($"Respuesta de la API RUC: {jsonString}");

                        return JObject.Parse(jsonString);
                    }
                    else
                    {
                        // Loguear el error si la respuesta no es exitosa
                        Console.WriteLine($"Error al consultar la API de RUC: {response.StatusCode}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Loguear cualquier excepción
                Console.WriteLine($"Error al consultar la API de RUC: {ex.Message}");
                return null;
            }
        }

        private JObject ConsultarApiDni(string numero)
        {
            try
            {
                string token = "apis-token-12635.jyQnRC3L3MHYAGvpHi8zHj3qRKJKYmkB";
                string apiUrl = $"https://api.apis.net.pe/v2/reniec/dni?numero={numero}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var response = client.GetAsync(apiUrl).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = response.Content.ReadAsStringAsync().Result;
                        // Loguear la respuesta para ver qué se está recibiendo
                        Console.WriteLine($"Respuesta de la API: {jsonString}");

                        return JObject.Parse(jsonString);
                    }
                    else
                    {
                        // Loguear el error si la respuesta no es exitosa
                        Console.WriteLine($"Error al consultar la API de DNI: {response.StatusCode}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Loguear cualquier excepción
                Console.WriteLine($"Error al consultar la API de DNI: {ex.Message}");
                return null;
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
