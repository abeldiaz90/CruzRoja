using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class PacientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            Contexto con = new Contexto();
            IEnumerable<Pacientes> p = con.pacientes.ToList();
            return View(p);
        }

        [HttpGet]
        public ActionResult Nuevo()
        {
            return View();
        }

        public ActionResult Busqueda()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Editar(Pacientes pa)
        {
            Contexto con = new Contexto();
            Pacientes p = con.pacientes.FirstOrDefault(model=>model.Id==pa.Id);
            p.pacientes = con.pacientes.ToList();
            return View(p);
        }
        public enum ValoresOpcion
        {
            Gestion = 1, Colegio = 2, Estado = 3, Pais = 4
        }
        [HttpPost]
        public ActionResult Buscar(Pacientes pa)
        {
            Contexto con = new Contexto();
            Pacientes p = con.pacientes.FirstOrDefault(model => model.RFC == pa.RFC);
            return View(p);
        }

        [HttpPost]
        public ActionResult Guardar(Pacientes paciente)
        {
            if (ModelState.IsValid)
            {
                Contexto contexto = new Contexto();
                var p = contexto.pacientes.FirstOrDefault(model => model.Id == paciente.Id);
                if (p == null)
                {
                    contexto.pacientes.Add(paciente);
                    contexto.SaveChanges();
                }
                else {
                    contexto.Set<Pacientes>().AddOrUpdate(paciente);
                    contexto.SaveChanges();
                }
            }
            return RedirectToAction("index");
        }
    }
}