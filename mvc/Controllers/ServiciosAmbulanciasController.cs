using Microsoft.Ajax.Utilities;
using mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;

namespace mvc.Controllers
{
    public class ServiciosAmbulanciasController : Controller
    {
        private Contexto db = new Contexto();
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Index()
        {
            IEnumerable<ServiciosAmbulancias> serviciosAmbulancias = db.ServiciosAmbulancias.ToList();
            foreach (var SerAmb in serviciosAmbulancias)
            {
                SerAmb.Calle = Seguridad.Decrypt(SerAmb.Calle);
                SerAmb.Cruzamientos = Seguridad.Decrypt(SerAmb.Cruzamientos);
                SerAmb.Referencia = Seguridad.Decrypt(SerAmb.Referencia);
                SerAmb.Reportante = Seguridad.Decrypt(SerAmb.Reportante);
                SerAmb.Telefono = Seguridad.Decrypt(SerAmb.Telefono);
                switch (SerAmb.Status)
                {
                    case "step-1":
                        SerAmb.Status = Resources.Recursos.SERVICIOS_AMBULANCIAS_STEP1;
                        break;
                    case "step-2":
                        SerAmb.Status = Resources.Recursos.SERVICIOS_AMBULANCIAS_STEP2;
                        break;
                    case "step-3":
                        SerAmb.Status = Resources.Recursos.SERVICIOS_AMBULANCIAS_STEP3;
                        break;
                    case "step-4":
                        SerAmb.Status = Resources.Recursos.SERVICIOS_AMBULANCIAS_STEP4;
                        break;
                    case "step-5":
                        SerAmb.Status = Resources.Recursos.SERVICIOS_AMBULANCIAS_STEP5;
                        break;
                    default:
                        SerAmb.Status = Resources.Recursos.SERVICIOS_AMBULANCIAS_STEP6;
                        break;
                }

            }
            IEnumerable<Colonias> colonias = db.Colonias.ToList();
            foreach (var colonia in colonias)
            {
                colonia.Colonia = Seguridad.Decrypt(colonia.Colonia);
            }
            IEnumerable<Claves> claves = db.Claves.ToList();
            foreach (var clave in claves)
            {
                clave.Descripcion = Seguridad.Decrypt(clave.Descripcion);
            }
            IEnumerable<Ambulancias> ambulancias = db.Ambulancias.ToList();
            foreach (var ambulancia in ambulancias)
            {
                ambulancia.Unidad = Seguridad.Decrypt(ambulancia.Unidad);
            }
            var vistaReportesDiarios = from sam in serviciosAmbulancias
                                       join col in colonias on sam.IdColonia equals col.IdColonia
                                       join cla in claves on sam.IdClave equals cla.IdClave
                                       join amb in ambulancias on sam.IdAmbulancia equals amb.IdAmbulancia
                                       select new ServiciosAmbulanciasVistas { serviciosAmbulancias = sam, colonias = col, claves = cla, ambulancias = amb };         

            return View(await Task.FromResult(vistaReportesDiarios.OrderByDescending(x => x.serviciosAmbulancias.Id)).ConfigureAwait(true));
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
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
            else
            {
                serviciosAmbulancias.Calle = Seguridad.Decrypt(serviciosAmbulancias.Calle);
                serviciosAmbulancias.Cruzamientos = Seguridad.Decrypt(serviciosAmbulancias.Cruzamientos);
                serviciosAmbulancias.Referencia = Seguridad.Decrypt(serviciosAmbulancias.Referencia);
                serviciosAmbulancias.Reportante = Seguridad.Decrypt(serviciosAmbulancias.Reportante);
                serviciosAmbulancias.Telefono = Seguridad.Decrypt(serviciosAmbulancias.Telefono);
            }
            return View(serviciosAmbulancias);
        }

        // GET: ServiciosAmbulancias/Create
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Create()
        {
            ServiciosAmbulancias serviciosAmbulancias = new ServiciosAmbulancias();
            IEnumerable<Colonias> colonias = db.Colonias.ToList();
            foreach (var colonia in colonias)
            {
                colonia.Colonia = Seguridad.Decrypt(colonia.Colonia);
            }
            IEnumerable<Claves> claveslista = db.Claves.ToList();
            foreach (var claves in claveslista)
            {
                claves.Clave = Seguridad.Decrypt(claves.Clave);
                claves.Descripcion = Seguridad.Decrypt(claves.Descripcion);
            }
            IEnumerable<Ambulancias> ambulanciaslista = db.Ambulancias.ToList();
            foreach (var ambulancia in ambulanciaslista)
            {
                ambulancia.Unidad = Seguridad.Decrypt(ambulancia.Unidad);
            }
            IEnumerable<Traslados> traslados = db.Traslados.ToList();
            foreach (var tr in traslados)
            {
                tr.Clave = Seguridad.Decrypt(tr.Clave);
                tr.Descripcion = Seguridad.Decrypt(tr.Descripcion);
            }
            serviciosAmbulancias.traslados = traslados;
            serviciosAmbulancias.colonias = colonias;
            serviciosAmbulancias.claves = claveslista;
            serviciosAmbulancias.ambulancias = ambulanciaslista;
            return View(await Task.FromResult(serviciosAmbulancias).ConfigureAwait(true));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Fecha,Reportante,Calle,NumeroExt,IdColonia,Cruzamientos,Referencia,Telefono,IdClave,HoraRecepcion,IdAmbulancia,KilometrajeSalida,HoraSalida,HoraLlegadaSitio,HoraTraslado,IdTraslado,HoraSalidaTraslado,KilometrajeLlegada,KilometrajeRecorrido,Status")] ServiciosAmbulancias serviciosAmbulancias)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    serviciosAmbulancias.Calle = Seguridad.Encrypt(serviciosAmbulancias.Calle);
                    serviciosAmbulancias.Cruzamientos = Seguridad.Encrypt(serviciosAmbulancias.Cruzamientos);
                    serviciosAmbulancias.Referencia = Seguridad.Encrypt(serviciosAmbulancias.Referencia);
                    serviciosAmbulancias.Reportante = Seguridad.Encrypt(serviciosAmbulancias.Reportante);
                    serviciosAmbulancias.Telefono = Seguridad.Encrypt(serviciosAmbulancias.Telefono);
                    serviciosAmbulancias.Status = "step-1";
                    db.ServiciosAmbulancias.Add(serviciosAmbulancias);
                    await db.SaveChangesAsync();
                    var id = serviciosAmbulancias.Id;
                    return RedirectToAction("Edit/" + id);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {

                    }
                    throw;
                }
            }
            return View(serviciosAmbulancias);
        }

        // GET: ServiciosAmbulancias/Edit/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
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
            else
            {
                IEnumerable<Colonias> colonias = db.Colonias.ToList();
                foreach (var colonia in colonias)
                {
                    colonia.Colonia = Seguridad.Decrypt(colonia.Colonia);
                }
                IEnumerable<Claves> claveslista = db.Claves.ToList();
                foreach (var claves in claveslista)
                {
                    claves.Clave = Seguridad.Decrypt(claves.Clave);
                    claves.Descripcion = Seguridad.Decrypt(claves.Descripcion);
                }
                IEnumerable<Ambulancias> ambulanciaslista = db.Ambulancias.ToList();
                foreach (var ambulancia in ambulanciaslista)
                {
                    ambulancia.Unidad = Seguridad.Decrypt(ambulancia.Unidad);
                }
                IEnumerable<Traslados> traslados = db.Traslados.ToList();
                foreach (var tr in traslados)
                {
                    tr.Clave = Seguridad.Decrypt(tr.Clave);
                    tr.Descripcion = Seguridad.Decrypt(tr.Descripcion);
                }
                serviciosAmbulancias.traslados = traslados;
                serviciosAmbulancias.Calle = Seguridad.Decrypt(serviciosAmbulancias.Calle);
                serviciosAmbulancias.Cruzamientos = Seguridad.Decrypt(serviciosAmbulancias.Cruzamientos);
                serviciosAmbulancias.Referencia = Seguridad.Decrypt(serviciosAmbulancias.Referencia);
                serviciosAmbulancias.Reportante = Seguridad.Decrypt(serviciosAmbulancias.Reportante);
                serviciosAmbulancias.Telefono = Seguridad.Decrypt(serviciosAmbulancias.Telefono);
                serviciosAmbulancias.colonias = colonias;
                serviciosAmbulancias.claves = claveslista;
                serviciosAmbulancias.ambulancias = ambulanciaslista;
            }
            return View(serviciosAmbulancias);
        }

        // POST: ServiciosAmbulancias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Fecha,Reportante,Calle,NumeroExt,IdColonia,Cruzamientos,Referencia,Telefono,IdClave,HoraRecepcion,IdAmbulancia,KilometrajeSalida,HoraSalida,HoraLlegadaSitio,HoraTraslado,IdTraslado,HoraSalidaTraslado,KilometrajeLlegada,KilometrajeRecorrido,Status")] ServiciosAmbulancias serviciosAmbulancias)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    serviciosAmbulancias.Calle = Seguridad.Encrypt(serviciosAmbulancias.Calle);
                    serviciosAmbulancias.Cruzamientos = Seguridad.Encrypt(serviciosAmbulancias.Cruzamientos);
                    serviciosAmbulancias.Referencia = Seguridad.Encrypt(serviciosAmbulancias.Referencia);
                    serviciosAmbulancias.Reportante = Seguridad.Encrypt(serviciosAmbulancias.Reportante);
                    serviciosAmbulancias.Telefono = Seguridad.Encrypt(serviciosAmbulancias.Telefono);
                    db.Entry(serviciosAmbulancias).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Edit/" + serviciosAmbulancias.Id);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {

                    }
                    throw;
                }
            }
            return View(serviciosAmbulancias);
        }

        // GET: ServiciosAmbulancias/Delete/5
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
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
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ServiciosAmbulancias serviciosAmbulancias = await db.ServiciosAmbulancias.FindAsync(id);
            db.ServiciosAmbulancias.Remove(serviciosAmbulancias);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Centralista")]
        public ActionResult ConsultaKilometraje(int? id)
        {
            dynamic kilometraje = 0;
            try
            {
                kilometraje = db.ServiciosAmbulancias.OrderByDescending(p => p.Id).Where(x => x.IdAmbulancia == id).FirstOrDefault().KilometrajeLlegada;
            }
            catch (Exception ex)
            {
                kilometraje = 0;
            }
            return Json(kilometraje, JsonRequestBehavior.AllowGet);
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
