using mvc.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
namespace mvc.Controllers
{
    public class ServiciosController : Controller
    {
        Contexto con = new Contexto();
        // GET: Servicios
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Servicios> servicios = con.servicios.ToList();
            List<Servicios> listaservicios = new List<Servicios>();
            foreach (var i in servicios)
            {
                Servicios ser = new Servicios();
                ser.Id = i.Id;
                ser.Clave = Seguridad.Decrypt(i.Clave);
                ser.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                listaservicios.Add(ser);
            }
            return View(listaservicios);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public ActionResult Editar(Servicios servicios)
        {
            var estadoslistado = con.servicios.FirstOrDefault(model => model.Id == servicios.Id);
            Servicios servicioslista = new Servicios();
            servicioslista.Id = estadoslistado.Id;
            servicioslista.Clave = Seguridad.Decrypt(estadoslistado.Clave);
            servicioslista.NombreServicio = Seguridad.Decrypt(estadoslistado.NombreServicio);
            return View(servicioslista);
        }

        [HttpPost]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Guardar(Servicios servicios)
        {
            if (ModelState.IsValid)
            {
                Servicios ser = con.servicios.FirstOrDefault(model => model.Id == servicios.Id);
                servicios.Id = servicios.Id;
                servicios.Clave = Seguridad.Encrypt(servicios.Clave);
                servicios.NombreServicio = Seguridad.Encrypt(servicios.NombreServicio);
                if (ser == null)
                {
                    con.servicios.Add(servicios);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<Servicios>().AddOrUpdate(servicios);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}