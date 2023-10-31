using MarketASP.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MarketASP.Areas.Inventario.Controllers
{
    [Authorize]
    public class KARDEXesController : Controller
    {
        private MarketWebEntities db = new MarketWebEntities();

        // GET: KARDEXes
        public async Task<ActionResult> Index()
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0205", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var resultado = db.Pr_KardexMovimientos("","","").ToList();

            return View(resultado);
        }
        public async Task<ActionResult> Resumen(string sdesc_arti,string sdesc_alma)
        {
            int xvalue = 0;
            ObjectParameter xcode = new ObjectParameter("xcode", typeof(int));

            db.Pr_PermisoAcceso(User.Identity.Name, "0206", xcode);
            xvalue = int.Parse(xcode.Value.ToString());
            if (xvalue == 0)
            {
                ViewBag.mensaje = "No tiene acceso, comuniquese con el administrador del sistema";
                return View("_Mensaje");
            }

            var kARDEX = db.Pr_KardexArticulos(0,sdesc_arti, sdesc_alma,0).ToList();
            return View(kARDEX);
        }
        // GET: KARDEXes/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            return View(kARDEX);
        }

        // GET: KARDEXes/Create
        public ActionResult Create()
        {
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma");
            return View();
        }

        // POST: KARDEXes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ncode_kardex,dfekard_kardex,stipomovi_kardex,ncant_kardex,npuco_kardex,ntc_kardex,smone_kardex,npucoMN_kardex,npucoUS_kardex,ncodeDoc_kardex,sserie_kardex,snume_kardex,dfvence_kardex,suser_kardex,dfech_kardex,susmo_kardex,dfemo_kardex,ncode_alma")] KARDEX kARDEX)
        {
            if (ModelState.IsValid)
            {
                db.KARDEX.Add(kARDEX);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        // GET: KARDEXes/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        // POST: KARDEXes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ncode_kardex,dfekard_kardex,stipomovi_kardex,ncant_kardex,npuco_kardex,ntc_kardex,smone_kardex,npucoMN_kardex,npucoUS_kardex,ncodeDoc_kardex,sserie_kardex,snume_kardex,dfvence_kardex,suser_kardex,dfech_kardex,susmo_kardex,dfemo_kardex,ncode_alma")] KARDEX kARDEX)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kARDEX).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ncode_alma = new SelectList(db.ALMACEN, "ncode_alma", "sdesc_alma", kARDEX.ncode_alma);
            return View(kARDEX);
        }

        // GET: KARDEXes/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            if (kARDEX == null)
            {
                return HttpNotFound();
            }
            return View(kARDEX);
        }

        // POST: KARDEXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            KARDEX kARDEX = await db.KARDEX.FindAsync(id);
            db.KARDEX.Remove(kARDEX);
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
