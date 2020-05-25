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
    public class TrasladosController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Traslados
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Traslados> traslados = db.Traslados.ToList();
            foreach (var t in traslados)
            {
                t.Clave = Seguridad.Decrypt(t.Clave);
                t.Descripcion = Seguridad.Decrypt(t.Descripcion);
            }
            return View(await Task.FromResult(traslados));
        }

        // GET: Traslados/Details/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Traslados traslados = await db.Traslados.FindAsync(id);
            if (traslados == null)
            {
                return HttpNotFound();
            }
            else
            {
                traslados.Clave = Seguridad.Decrypt(traslados.Clave);
                traslados.Descripcion = Seguridad.Decrypt(traslados.Descripcion);
            }
            return View(traslados);
        }

        // GET: Traslados/Create
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Traslados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Clave,Descripcion")] Traslados traslados)
        {
            if (ModelState.IsValid)
            {
                traslados.Clave = Seguridad.Encrypt(traslados.Clave);
                traslados.Descripcion = Seguridad.Encrypt(traslados.Descripcion);
                db.Traslados.Add(traslados);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(traslados);
        }

        // GET: Traslados/Edit/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Traslados traslados = await db.Traslados.FindAsync(id);
            if (traslados == null)
            {
                return HttpNotFound();
            }
            else
            {
                traslados.Clave = Seguridad.Decrypt(traslados.Clave);
                traslados.Descripcion = Seguridad.Decrypt(traslados.Descripcion);
            }
            return View(traslados);
        }

        // POST: Traslados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Clave,Descripcion")] Traslados traslados)
        {
            if (ModelState.IsValid)
            {
                traslados.Clave = Seguridad.Encrypt(traslados.Clave);
                traslados.Descripcion = Seguridad.Encrypt(traslados.Descripcion);
                db.Entry(traslados).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(traslados);
        }

        // GET: Traslados/Delete/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Traslados traslados = await db.Traslados.FindAsync(id);
            if (traslados == null)
            {
                return HttpNotFound();
            }
            return View(traslados);
        }

        // POST: Traslados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Traslados traslados = await db.Traslados.FindAsync(id);
            db.Traslados.Remove(traslados);
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
