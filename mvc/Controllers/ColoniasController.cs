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
    public class ColoniasController : Controller
    {
        private Contexto db = new Contexto();

        // GET: Colonias
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Colonias> colonias = db.Colonias.ToList();
            foreach (var col in colonias) 
            {
                col.Colonia = Seguridad.Decrypt(col.Colonia);
            }
            return View(await Task.FromResult(colonias));
           // return View(await db.Colonias.ToListAsync());
        }

        // GET: Colonias/Details/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colonias colonias = await db.Colonias.FindAsync(id);
            if (colonias == null)
            {
                return HttpNotFound();
            }
            else 
            {
                colonias.Colonia = Seguridad.Decrypt(colonias.Colonia);
            }
            return View(colonias);
        }

        // GET: Colonias/Create
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Colonias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Colonia")] Colonias colonias)
        {
            if (ModelState.IsValid)
            {
                colonias.Colonia = Seguridad.Encrypt(colonias.Colonia);
                db.Colonias.Add(colonias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(colonias);
        }

        // GET: Colonias/Edit/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colonias colonias = await db.Colonias.FindAsync(id);
            if (colonias == null)
            {
                return HttpNotFound();
            }
            else 
            {
                colonias.Colonia = Seguridad.Decrypt(colonias.Colonia);
            }            
            return View(colonias);
        }

        // POST: Colonias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Colonia")] Colonias colonias)
        {
            if (ModelState.IsValid)
            {
                colonias.Colonia = Seguridad.Encrypt(colonias.Colonia);
                db.Entry(colonias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(colonias);
        }

        // GET: Colonias/Delete/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Colonias colonias = await db.Colonias.FindAsync(id);
            if (colonias == null)
            {
                return HttpNotFound();
            }
            return View(colonias);
        }

        // POST: Colonias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Colonias colonias = await db.Colonias.FindAsync(id);
            db.Colonias.Remove(colonias);
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
