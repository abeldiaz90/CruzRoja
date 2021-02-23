using mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
namespace mvc.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly int _RegistrosPorPagina = 10;
        Contexto con = new Contexto();
        private List<Servicios> _Servicios;
        private Paginador<Servicios> _PaginadorServicios;
        // GET: Servicios
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(int pagina = 1)
        {
            int _TotalRegistros = 0;
            _TotalRegistros = con.servicios.Count();
            _Servicios=con.servicios.OrderBy(x => x.NombreServicio)
                                                 .Skip((pagina - 1) * _RegistrosPorPagina)
                                                 .Take(_RegistrosPorPagina)
                                                 .ToList();
            foreach (var i in _Servicios)
            {               
                i.Id = i.Id;
                i.Clave = Seguridad.Decrypt(i.Clave);
                i.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
            }
            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);
            _PaginadorServicios = new Paginador<Servicios>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = _Servicios
            };           
            return View(_PaginadorServicios);
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