using Microsoft.Ajax.Utilities;
using mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
namespace mvc.Controllers
{
    public class ServiciosDelegacionController : Controller
    {
        private readonly int _RegistrosPorPagina = 10;
        private List<ServiciosDelegacion> _ServiciosDelegacion;
        private Paginador<ServiciosVista> _Paginador;
        Contexto con = new Contexto();
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(int pagina = 1)
        {
            int _TotalRegistros = 0;
            _TotalRegistros = con.serviciosdelegacion.Count();
            _ServiciosDelegacion = con.serviciosdelegacion.OrderBy(x => x.NombreServicio)
                                                 .Skip((pagina - 1) * _RegistrosPorPagina)
                                                 .Take(_RegistrosPorPagina)
                                                 .ToList();

            var listaunicaservicios= _ServiciosDelegacion.DistinctBy(x => x.IdServicio).ToList();
            IEnumerable<Servicios> serviciosvista = con.servicios.Where(x => x.Id.IsAnyOf(_ServiciosDelegacion.Select(y => y.IdServicio).ToArray())).ToList();
            var vistaservicios = from sd in _ServiciosDelegacion
                                 join ss in serviciosvista on sd.IdServicio equals ss.Id
                                 select new ServiciosVista { servicios = ss, serviciosdelegacion = sd };


            foreach (var i in vistaservicios)
            {
                i.servicios.Clave = Seguridad.Decrypt(i.servicios.Clave);
                i.servicios.NombreServicio = Seguridad.Decrypt(i.servicios.NombreServicio);
                i.serviciosdelegacion.NombreServicio = Seguridad.Decrypt(i.serviciosdelegacion.NombreServicio);
            }

            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
            _Paginador = new Paginador<ServiciosVista>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = vistaservicios.OrderBy(c=>c.servicios.NombreServicio)
            };
            return View(_Paginador);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
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
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Editar(ServiciosDelegacion serviciosdelegacion)
        {
            ServiciosDelegacion serviciosDelegacion = con.serviciosdelegacion.FirstOrDefault(model => model.Id == serviciosdelegacion.Id);
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
            serviciosDelegacion.NombreServicio = Seguridad.Decrypt(serviciosDelegacion.NombreServicio);
            serviciosDelegacion.AplicaIVA = serviciosDelegacion.AplicaIVA;
            return View(serviciosDelegacion);
        }

        [HttpPost]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
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
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador,Capturista")]
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


            IEnumerable<ServiciosDelegacionPrecios> sdp = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == id);
            return PartialView("Precios", vistatotal.Where(s => s.serviciosdelegacionprecios.IdServicio == id));
        }
    }
}