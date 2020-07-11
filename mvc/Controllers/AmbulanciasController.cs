using mvc.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace mvc.Controllers
{
    public class AmbulanciasController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Ambulancias
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Ambulancias> ambulancias = db.Ambulancias.ToList();
            foreach (var i in ambulancias)
            {
                i.Unidad = Seguridad.Decrypt(i.Unidad);
            }
            return View(ambulancias.ToList());
        }

        // GET: Ambulancias/Details/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulancias ambulancias = await db.Ambulancias.FindAsync(id);
            if (ambulancias == null)
            {
                return HttpNotFound();
            }
            ambulancias.Unidad = Seguridad.Decrypt(ambulancias.Unidad);
            return View(ambulancias);
        }

        // GET: Ambulancias/Create
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ambulancias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create([Bind(Include = "IdAmbulancia,Unidad")] Ambulancias ambulancias)
        {
            if (ModelState.IsValid)
            {
                ambulancias.Unidad = Seguridad.Encrypt(ambulancias.Unidad);
                db.Ambulancias.Add(ambulancias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(ambulancias);
        }

        // GET: Ambulancias/Edit/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulancias ambulancias = await db.Ambulancias.FindAsync(id);
            if (ambulancias == null)
            {
                return HttpNotFound();
            }
            else
            {
                ambulancias.Unidad = Seguridad.Decrypt(ambulancias.Unidad);
            }
            return View(ambulancias);
        }

        // POST: Ambulancias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit([Bind(Include = "IdAmbulancia,Unidad")] Ambulancias ambulancias)
        {
            if (ModelState.IsValid)
            {
                ambulancias.Unidad = Seguridad.Encrypt(ambulancias.Unidad);
                db.Entry(ambulancias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ambulancias);
        }

        // GET: Ambulancias/Delete/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulancias ambulancias = await db.Ambulancias.FindAsync(id);
            if (ambulancias == null)
            {
                return HttpNotFound();
            }
            return View(ambulancias);
        }

        // POST: Ambulancias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Ambulancias ambulancias = await db.Ambulancias.FindAsync(id);
            db.Ambulancias.Remove(ambulancias);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
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
