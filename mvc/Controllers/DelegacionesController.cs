using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class DelegacionesController : Controller
    {
        // GET: Estados
        // GET: Paises
        Contexto con = new Contexto();
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Estados> estados = con.estados.ToList();
            IEnumerable<Delegaciones> delegaciones = con.delegaciones.ToList();
            var d = from del in delegaciones
                    join est in estados on del.IdEstado equals est.Id
                    select new DelegacionesVista { estados = est, delegaciones = del };
            return View(d);
        }
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Nuevo()
        {
            Delegaciones delegacioneslistado = new Delegaciones();
            delegacioneslistado.estado = con.estados.ToList();
            List<Estados> estados = new List<Estados>();
            foreach (var i in delegacioneslistado.estado)
            {
                Estados p = new Estados();
                p.Id = i.Id;
                p.Estado = Seguridad.Decrypt(i.Estado);
                estados.Add(p);
            }
            delegacioneslistado.estado = estados;

            return View(delegacioneslistado);
        }

        [HttpGet]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Editar(Delegaciones delegaciones)
        {
            Delegaciones delegacioneslistado = con.delegaciones.FirstOrDefault(model => model.Id == delegaciones.Id);
            delegacioneslistado.Calle = Seguridad.Decrypt(delegacioneslistado.Calle);
            delegacioneslistado.claveDelegacion = Seguridad.Decrypt(delegacioneslistado.claveDelegacion);
            delegacioneslistado.Colonia = Seguridad.Decrypt(delegacioneslistado.Colonia);
            delegacioneslistado.CP = Seguridad.Decrypt(delegacioneslistado.CP);
            delegacioneslistado.Municipio = Seguridad.Decrypt(delegacioneslistado.Municipio);
            delegacioneslistado.NumeroExterior = Seguridad.Decrypt(delegacioneslistado.NumeroExterior);
            delegacioneslistado.NumeroInterior = Seguridad.Decrypt(delegacioneslistado.NumeroInterior);
            delegacioneslistado.Telefono = Seguridad.Decrypt(delegacioneslistado.Telefono);
            delegacioneslistado.estado = con.estados.ToList();

            List<Estados> estados = new List<Estados>();
            foreach (var i in delegacioneslistado.estado)
            {
                Estados p = new Estados();
                p.Id = i.Id;
                p.Estado = Seguridad.Decrypt(i.Estado);
                estados.Add(p);
            }
            delegacioneslistado.estado = estados;

            return View(delegacioneslistado);
        }

        [HttpPost]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Guardar(Delegaciones delegaciones)
        {
            if (ModelState.IsValid)
            {
                Delegaciones delegacioneslistado = con.delegaciones.FirstOrDefault(model => model.Id == delegaciones.Id);
                
                if (delegacioneslistado == null)
                {
                    Delegaciones del = new Delegaciones();
                    del.claveDelegacion = Seguridad.Encrypt(delegaciones.claveDelegacion);
                    del.Calle = Seguridad.Encrypt(delegaciones.Calle);
                    del.Colonia = Seguridad.Encrypt(delegaciones.Colonia);
                    del.CP = Seguridad.Encrypt(delegaciones.CP);
                    del.Municipio = Seguridad.Encrypt(delegaciones.Municipio);
                    del.Telefono = Seguridad.Encrypt(delegaciones.Telefono);
                    del.NumeroExterior = Seguridad.Encrypt(delegaciones.NumeroExterior);
                    del.NumeroInterior = Seguridad.Encrypt(delegaciones.NumeroInterior);
                    del.IdEstado = delegaciones.IdEstado;
                    con.delegaciones.Add(del);
                    con.SaveChanges();
                }
                else
                {
                    delegacioneslistado.claveDelegacion = Seguridad.Encrypt(delegaciones.claveDelegacion);
                    delegacioneslistado.Calle = Seguridad.Encrypt(delegaciones.Calle);
                    delegacioneslistado.Colonia = Seguridad.Encrypt(delegaciones.Colonia);
                    delegacioneslistado.CP = Seguridad.Encrypt(delegaciones.CP);
                    delegacioneslistado.Municipio = Seguridad.Encrypt(delegaciones.Municipio);
                    delegacioneslistado.Telefono = Seguridad.Encrypt(delegaciones.Telefono);
                    delegacioneslistado.NumeroExterior = Seguridad.Encrypt(delegaciones.NumeroExterior);
                    delegacioneslistado.NumeroInterior = Seguridad.Encrypt(delegaciones.NumeroInterior);
                    delegacioneslistado.IdEstado = delegaciones.IdEstado;
                    con.Set<Delegaciones>().AddOrUpdate(delegacioneslistado);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}