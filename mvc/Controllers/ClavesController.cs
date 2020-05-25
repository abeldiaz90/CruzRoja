using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class ClavesController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Claves
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Claves> claves = db.Claves.ToList();
            List<Claves> listaclaves = new List<Claves>();
            foreach (var clave in claves)
            {
                clave.Clave = Seguridad.Decrypt(clave.Clave);
                clave.Descripcion = Seguridad.Decrypt(clave.Descripcion);
                listaclaves.Add(clave);
            }
       
         
            // return View(await db.Claves.ToListAsync());
            return View(await Task.FromResult(listaclaves));
        }

        // GET: Claves/Details/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claves claves = await db.Claves.FindAsync(id);
            if (claves == null)
            {
                return HttpNotFound();
            }
            else
            {
                claves.Clave = Seguridad.Decrypt(claves.Clave);
                claves.Descripcion = Seguridad.Decrypt(claves.Descripcion);
            }
            return View(claves);
        }

        // GET: Claves/Create
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Claves/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Clave,Descripcion")] Claves claves)
        {
            if (ModelState.IsValid)
            {
                claves.Clave = Seguridad.Encrypt(claves.Clave);
                claves.Descripcion = Seguridad.Encrypt(claves.Descripcion);
                db.Claves.Add(claves);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(claves);
        }

        // GET: Claves/Edit/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claves claves = await db.Claves.FindAsync(id);
            if (claves == null)
            {
                return HttpNotFound();
            }
            else
            {
                claves.Clave = Seguridad.Decrypt(claves.Clave);
                claves.Descripcion = Seguridad.Decrypt(claves.Descripcion);
            }
            return View(claves);
        }

        // POST: Claves/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Clave,Descripcion")] Claves claves)
        {
            if (ModelState.IsValid)
            {
                claves.Clave = Seguridad.Encrypt(claves.Clave);
                claves.Descripcion = Seguridad.Encrypt(claves.Descripcion);
                db.Entry(claves).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(claves);
        }

        // GET: Claves/Delete/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claves claves = await db.Claves.FindAsync(id);
            if (claves == null)
            {
                return HttpNotFound();
            }
            return View(claves);
        }

        // POST: Claves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Claves claves = await db.Claves.FindAsync(id);
            db.Claves.Remove(claves);
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
