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
            ServiciosDelegacion serviciosDelegacion = new ServiciosDelegacion();
            IEnumerable<ServiciosDelegacion> serviciosDelegacions = con.serviciosdelegacion.ToList();
            IEnumerable<Servicios> serviciosvista = con.servicios.ToList();
            List<Servicios> servicios = new List<Servicios>();
            foreach (var i in serviciosvista)
            {
                Servicios s = new Servicios();
                s.Id = i.Id;
                s.Clave = Seguridad.Decrypt(i.Clave);
                s.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                servicios.Add(s);
            }

            List<ServiciosDelegacion> serviciosdelegacion = new List<ServiciosDelegacion>();
            foreach (var i in serviciosDelegacions)
            {
                ServiciosDelegacion s = new ServiciosDelegacion();
                s.Id = i.Id;
                s.IdDelegacion = i.IdDelegacion;
                s.IdServicio = i.IdServicio;
                s.NombreServicio= Seguridad.Decrypt(i.NombreServicio);
                s.precios = i.precios;
                s.servicios = i.servicios;
                s.serviciosdelegacionvista = i.serviciosdelegacionvista;
                serviciosdelegacion.Add(s);
            }



            var vistaservicios = from sd in serviciosdelegacion
                                 join ss in servicios on sd.IdServicio equals ss.Id
                                 select new ServiciosVista { servicios = ss, serviciosdelegacion = sd };
            serviciosDelegacion.serviciosdelegacionvista = vistaservicios.OrderBy(s=>s.serviciosdelegacion.NombreServicio);
         
            //foreach (var i in vistaservicios)
            //{
            //    ServiciosVista sv = new ServiciosVista();
            //    sv.servicios.Id = i.servicios.Id;
            //    sv.servicios.Clave = Seguridad.Decrypt(i.servicios.Clave);
            //    sv.servicios.NombreServicio = Seguridad.Decrypt(i.servicios.NombreServicio);
            //    vistaservicios
            //}

            serviciosDelegacion.precios = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == 20);
            return View(serviciosDelegacion);
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
            serviciosDelegacion.NombreServicio = Seguridad.Decrypt(serviciosDelegacion.NombreServicio);
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

        [HttpGet]
        public ActionResult Precios(int id)
        {
            IEnumerable<ServiciosDelegacion> serviciosDelegacions = con.serviciosdelegacion.ToList();
            IEnumerable<Servicios> serviciosvista = con.servicios.ToList();
            IEnumerable<ServiciosDelegacionPrecios> serviciosdelegacionPrecios = con.serviciosDelegacionPrecios.ToList();
            var vistaservicios = from sd in serviciosDelegacions
                                 join ss in serviciosvista on sd.IdServicio equals ss.Id
                                 select new ServiciosVista { servicios = ss, serviciosdelegacion = sd };

            var vistatotal = from vista in vistaservicios
                             join vistaprecios in serviciosdelegacionPrecios on vista.serviciosdelegacion.IdServicio equals vistaprecios.IdServicio
                             select new ServiciosVista { servicios = vista.servicios, serviciosdelegacion = vista.serviciosdelegacion, serviciosdelegacionprecios = vistaprecios };


            IEnumerable <ServiciosDelegacionPrecios> sdp = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == id);
            return PartialView("Precios", vistatotal.Where(s => s.serviciosdelegacionprecios.IdServicio == id));
        }
    }
}