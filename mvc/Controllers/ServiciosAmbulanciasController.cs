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
    public class ServiciosAmbulanciasController : Controller
    {
        private Contexto db = new Contexto();

        // GET: ServiciosAmbulancias
        public async Task<ActionResult> Index()
        {
            return View(await db.ServiciosAmbulancias.ToListAsync());
        }

        // GET: ServiciosAmbulancias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiciosAmbulancias serviciosAmbulancias = await db.ServiciosAmbulancias.FindAsync(id);
            if (serviciosAmbulancias == null)
            {
                return HttpNotFound();
            }
            return View(serviciosAmbulancias);
        }

        // GET: ServiciosAmbulancias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServiciosAmbulancias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Fecha,Reportante,Calle,NumeroExt,IdColonia,Cruzamientos,Referencia,Telefono,ClaveEmergencia,HoraRecepcion,UnidadAsignada,KilometrajeSalida,HoraSalida")] ServiciosAmbulancias serviciosAmbulancias)
        {
            if (ModelState.IsValid)
            {
                db.ServiciosAmbulancias.Add(serviciosAmbulancias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(serviciosAmbulancias);
        }

        // GET: ServiciosAmbulancias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiciosAmbulancias serviciosAmbulancias = await db.ServiciosAmbulancias.FindAsync(id);
            if (serviciosAmbulancias == null)
            {
                return HttpNotFound();
            }
            return View(serviciosAmbulancias);
        }

        // POST: ServiciosAmbulancias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Fecha,Reportante,Calle,NumeroExt,IdColonia,Cruzamientos,Referencia,Telefono,ClaveEmergencia,HoraRecepcion,UnidadAsignada,KilometrajeSalida,HoraSalida")] ServiciosAmbulancias serviciosAmbulancias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(serviciosAmbulancias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(serviciosAmbulancias);
        }

        // GET: ServiciosAmbulancias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiciosAmbulancias serviciosAmbulancias = await db.ServiciosAmbulancias.FindAsync(id);
            if (serviciosAmbulancias == null)
            {
                return HttpNotFound();
            }
            return View(serviciosAmbulancias);
        }

        // POST: ServiciosAmbulancias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiciosAmbulancias serviciosAmbulancias = await db.ServiciosAmbulancias.FindAsync(id);
            db.ServiciosAmbulancias.Remove(serviciosAmbulancias);
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
