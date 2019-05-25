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
            List<Pacientes> pacienteslista = new List<Pacientes>();
            foreach (var i in p)
            {
                Pacientes pacientes = new Pacientes();
                pacientes.Nombre = Seguridad.Decrypt(i.Nombre);
                pacientes.SegundoNombre = Seguridad.Decrypt(i.Nombre);
                pacientes.ApellidoPaterno= Seguridad.Decrypt(i.ApellidoPaterno);
                pacientes.ApellidoMaterno = Seguridad.Decrypt(i.ApellidoMaterno);
                pacientes.RFC = Seguridad.Decrypt(i.RFC);
                pacientes.CURP = Seguridad.Decrypt(i.CURP);
                pacienteslista.Add(pacientes);
            }
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
            pa.Nombre = Seguridad.Decrypt(pa.ApellidoMaterno);
            pa.SegundoNombre = Seguridad.Decrypt(pa.SegundoNombre);
            pa.ApellidoPaterno = Seguridad.Decrypt(pa.ApellidoPaterno);
            pa.ApellidoMaterno = Seguridad.Decrypt(pa.ApellidoMaterno);
            pa.RFC = Seguridad.Decrypt(pa.RFC);
            pa.CURP = Seguridad.Decrypt(pa.CURP);
            pa.pacientes = con.pacientes.ToList();            
            return View(pa);
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
                paciente.Nombre = Seguridad.Encrypt(paciente.Nombre);
                paciente.SegundoNombre = Seguridad.Encrypt(paciente.SegundoNombre);
                paciente.ApellidoPaterno = Seguridad.Encrypt(paciente.ApellidoPaterno);
                paciente.ApellidoMaterno = Seguridad.Encrypt(paciente.ApellidoMaterno);
                paciente.RFC = Seguridad.Encrypt(paciente.RFC);
                paciente.CURP = Seguridad.Encrypt(paciente.CURP);
                paciente.pacientes = contexto.pacientes.ToList();
                var p = contexto.pacientes.FirstOrDefault(model => model.Id == paciente.Id);
                if (p == null)
                {
                    contexto.pacientes.Add(paciente);
                    contexto.SaveChanges();
                }
                else
                {
                    contexto.Set<Pacientes>().AddOrUpdate(paciente);
                    contexto.SaveChanges();
                }
            }
            return RedirectToAction("index");
        }
    }
}