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
            ServiciosDelegacion serviciosdelegacion = new ServiciosDelegacion();
            serviciosdelegacion.delegaciones = con.delegaciones.ToList();
            serviciosdelegacion.servicios = con.servicios.ToList();
            return View(serviciosdelegacion);
        }

        [HttpGet]
        public ActionResult Editar(ServiciosDelegacion serviciosdelegacion)
        {
            ServiciosDelegacion serviciosDelegacion = con.serviciosdelegacion.FirstOrDefault(model => model.Id == serviciosdelegacion.Id);
            serviciosDelegacion.delegaciones = con.delegaciones.ToList();
            serviciosDelegacion.servicios = con.servicios.ToList();
            return View(serviciosDelegacion);
        }

        [HttpPost]
        public ActionResult Guardar(ServiciosDelegacion serviciosdelegacion)
        {
            if (ModelState.IsValid)
            {
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