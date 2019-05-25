using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class ServiciosDelegacionController : Controller
    {
        Contexto con = new Contexto();
        public ActionResult Index()
        {
            IEnumerable<ServiciosDelegacion> serviciosDelegacions = con.serviciosdelegacion.ToList();
            IEnumerable<Servicios> serviciosvista = con.servicios.ToList();
            var vistaservicios = from sd in serviciosDelegacions
                                 join ss in serviciosvista on sd.IdServicio equals ss.Id
                                 select new ServiciosVista { servicios = ss, serviciosdelegacion = sd };
            return View(vistaservicios);
        }

        public ActionResult Nuevo()
        {
            ServiciosDelegacion serviciosDelegacion = new ServiciosDelegacion();
            serviciosDelegacion.delegaciones = con.delegaciones.ToList();

            List<Delegaciones> lista = new List<Delegaciones>();
            foreach (var i in serviciosDelegacion.delegaciones)
            {
                Delegaciones d = new Delegaciones();
                d.Id = i.Id;
                d.Municipio = Seguridad.Decrypt(i.Municipio);
                lista.Add(d);
            }

            serviciosDelegacion.servicios = con.servicios.ToList();
            List<Servicios> listaserviciosdelegacion = new List<Servicios>();
            foreach (var i in serviciosDelegacion.servicios)
            {
                Servicios ser = new Servicios();
                ser.Id = i.Id;
                ser.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                listaserviciosdelegacion.Add(ser);
            }
            serviciosDelegacion.delegaciones = lista;
            serviciosDelegacion.servicios = listaserviciosdelegacion;
            return View(serviciosDelegacion);
        }

        [HttpGet]
        public ActionResult Editar(ServiciosDelegacion serviciosdelegacion)
        {
            ServiciosDelegacion serviciosDelegacion = con.serviciosdelegacion.FirstOrDefault(model => model.Id == serviciosdelegacion.Id);
            serviciosDelegacion.delegaciones = con.delegaciones.ToList();

            List<Delegaciones> lista = new List<Delegaciones>();          
            foreach(var i in serviciosDelegacion.delegaciones)
            {
                Delegaciones d = new Delegaciones();
                d.Id = i.Id;
                d.Municipio = Seguridad.Decrypt(i.Municipio);
                lista.Add(d);
            }
         
            serviciosDelegacion.servicios = con.servicios.ToList();
            List<Servicios> listaserviciosdelegacion = new List<Servicios>();
            foreach (var i in serviciosDelegacion.servicios)
            {
                Servicios ser = new Servicios();
                ser.Id = i.Id;
                ser.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                listaserviciosdelegacion.Add(ser);
            }
            serviciosDelegacion.delegaciones = lista;
            serviciosDelegacion.servicios = listaserviciosdelegacion;
            return View(serviciosDelegacion);
        }

        [HttpPost]
        public ActionResult Guardar(ServiciosDelegacion serviciosdelegacion)
        {
            if (ModelState.IsValid)
            {
                serviciosdelegacion.NombreServicio = Seguridad.Encrypt(serviciosdelegacion.NombreServicio);
                ServiciosDelegacion delegacioneslistado = con.serviciosdelegacion.FirstOrDefault(model => model.Id == serviciosdelegacion.Id);
                if (delegacioneslistado == null)
                {

                    con.serviciosdelegacion.Add(serviciosdelegacion);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<ServiciosDelegacion>().AddOrUpdate(serviciosdelegacion);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}