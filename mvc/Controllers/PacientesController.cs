using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class PacientesController : Controller
    {
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Index()
        {
            Contexto con = new Contexto();
            IEnumerable<Pacientes> p = con.pacientes.ToList();
            List<Pacientes> pacienteslista = new List<Pacientes>();
            foreach (var i in p)
            {
                Pacientes pacientes = new Pacientes();
                pacientes.Id = i.Id;
                pacientes.Nombre = Seguridad.Decrypt(i.Nombre);
                pacientes.SegundoNombre = Seguridad.Decrypt(i.SegundoNombre);
                pacientes.ApellidoPaterno = Seguridad.Decrypt(i.ApellidoPaterno);
                pacientes.ApellidoMaterno = Seguridad.Decrypt(i.ApellidoMaterno);
                pacientes.RFC = Seguridad.Decrypt(i.RFC);
                pacientes.CURP = Seguridad.Decrypt(i.CURP);
                pacientes.Email = Seguridad.Decrypt(i.Email);
                pacientes.Telefono= i.Telefono;
                pacientes.FechaNacimiento = i.FechaNacimiento;
                pacienteslista.Add(pacientes);
            }
            return View(pacienteslista);
        }

        [HttpGet]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Nuevo()
        {
            return View();
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Busqueda()
        {
            return View();
        }

        [HttpGet]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Editar(Pacientes pa)
        {
            Contexto con = new Contexto();
            Pacientes p = con.pacientes.FirstOrDefault(s => s.Id == pa.Id);
            pa.Id = p.Id;
            pa.Nombre = Seguridad.Decrypt(p.Nombre);
            pa.SegundoNombre = Seguridad.Decrypt(p.SegundoNombre);
            pa.ApellidoPaterno = Seguridad.Decrypt(p.ApellidoPaterno);
            pa.ApellidoMaterno = Seguridad.Decrypt(p.ApellidoMaterno);
            pa.RFC = Seguridad.Decrypt(p.RFC);
            pa.CURP = Seguridad.Decrypt(p.CURP);
            pa.Sexo = p.Sexo;
            pa.FechaNacimiento = p.FechaNacimiento;
            pa.Email = Seguridad.Decrypt(p.Email);
            pa.Telefono = p.Telefono;
            pa.pacientes = con.pacientes.ToList();
            return View(pa);
        }
 
        [HttpPost]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public PartialViewResult Buscar(Pacientes pa)
        {
            Contexto con = new Contexto();
            IEnumerable<Pacientes> p = con.pacientes.ToList();
         
            List<Pacientes> pacientes = new List<Pacientes>();
            foreach (var i in p)
            {
                Pacientes paciente = new Pacientes();
                paciente.Id = i.Id;
                paciente.Nombre = Seguridad.Decrypt(i.Nombre);
                paciente.SegundoNombre = Seguridad.Decrypt(i.SegundoNombre);
                paciente.ApellidoPaterno = Seguridad.Decrypt(i.ApellidoPaterno);
                paciente.ApellidoMaterno = Seguridad.Decrypt(i.ApellidoMaterno);
                paciente.RFC = Seguridad.Decrypt(i.RFC);
                paciente.CURP = Seguridad.Decrypt(i.CURP);
                paciente.Email = Seguridad.Decrypt(i.Email);
                paciente.Telefono = i.Telefono;
                pacientes.Add(paciente);
            }
            if (string.IsNullOrEmpty(pa.Nombre))
            {
                pa.Nombre = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.Nombre.Contains(pa.Nombre.ToUpper())).ToList();
            }

            if (string.IsNullOrEmpty(pa.SegundoNombre))
            {
                pa.SegundoNombre = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.SegundoNombre.Contains(pa.SegundoNombre.ToUpper())).ToList();
            }

            if (string.IsNullOrEmpty(pa.ApellidoPaterno))
            {
                pa.ApellidoPaterno = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.ApellidoPaterno.Contains(pa.ApellidoPaterno.ToUpper())).ToList();
            }

            if (string.IsNullOrEmpty(pa.ApellidoMaterno))
            {
                pa.ApellidoMaterno = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.ApellidoMaterno.Contains(pa.ApellidoMaterno.ToUpper())).ToList();
            }

             return PartialView("Resultados", pacientes);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult BuscarId(Pacientes pa)
        {
            Contexto con = new Contexto();
            IEnumerable<Pacientes> p = con.pacientes.ToList().Where(s=>s.Id==pa.Id);

            List<Pacientes> pacientes = new List<Pacientes>();
            foreach (var i in p)
            {
                Pacientes paciente = new Pacientes();
                paciente.Id = i.Id;
                paciente.Nombre = Seguridad.Decrypt(i.Nombre);
                paciente.SegundoNombre = Seguridad.Decrypt(i.SegundoNombre);
                paciente.ApellidoPaterno = Seguridad.Decrypt(i.ApellidoPaterno);
                paciente.ApellidoMaterno = Seguridad.Decrypt(i.ApellidoMaterno);
                paciente.FechaNacimiento = i.FechaNacimiento;
                paciente.RFC = Seguridad.Decrypt(i.RFC);
                paciente.CURP = Seguridad.Decrypt(i.CURP);
                paciente.Email = Seguridad.Decrypt(i.Email);
                paciente.Telefono = i.Telefono;
                paciente.Sexo = i.Sexo;
                pacientes.Add(paciente);
            }
            if (string.IsNullOrEmpty(pa.Nombre))
            {
                pa.Nombre = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.Nombre.Contains(s.Nombre.ToUpper())).ToList();
            }

            if (string.IsNullOrEmpty(pa.SegundoNombre))
            {
                pa.SegundoNombre = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.SegundoNombre.Contains(pa.SegundoNombre.ToUpper())).ToList();
            }

            if (string.IsNullOrEmpty(pa.ApellidoPaterno))
            {
                pa.ApellidoPaterno = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.ApellidoPaterno.Contains(pa.ApellidoPaterno.ToUpper())).ToList();
            }

            if (string.IsNullOrEmpty(pa.ApellidoMaterno))
            {
                pa.ApellidoMaterno = string.Empty;
            }
            else
            {
                pacientes = pacientes.Where(s => s.ApellidoMaterno.Contains(pa.ApellidoMaterno.ToUpper())).ToList();
            }
            return Json(pacientes, JsonRequestBehavior.AllowGet);

           // return PartialView("Resultados", pacientes);
        }


        [HttpPost]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Guardar(Pacientes paciente)
        {
            if (ModelState.IsValid)
            {
                Contexto contexto = new Contexto();
                paciente.Id = paciente.Id;
                paciente.Nombre = Seguridad.Encrypt(paciente.Nombre.ToUpper());
                if (string.IsNullOrEmpty(paciente.SegundoNombre))
                {
                    paciente.SegundoNombre = paciente.SegundoNombre;
                }
                else
                {
                    paciente.SegundoNombre = Seguridad.Encrypt(paciente.SegundoNombre.ToUpper());
                }
       
                paciente.ApellidoPaterno = Seguridad.Encrypt(paciente.ApellidoPaterno.ToUpper());

                if (string.IsNullOrEmpty(paciente.ApellidoMaterno))
                {
                    paciente.ApellidoMaterno = paciente.ApellidoMaterno;
                }
                else
                {
                    paciente.ApellidoMaterno = Seguridad.Encrypt(paciente.ApellidoMaterno.ToUpper());
                }

                if (string.IsNullOrEmpty(paciente.RFC))
                {
                    paciente.RFC = paciente.RFC;
                }
                else
                {
                    paciente.RFC = Seguridad.Encrypt(paciente.RFC.ToUpper());
                }

                if (string.IsNullOrEmpty(paciente.CURP))
                {
                    paciente.CURP = paciente.CURP;
                }
                else
                {
                    paciente.CURP = Seguridad.Encrypt(paciente.CURP.ToUpper());
                }

                if (string.IsNullOrEmpty(paciente.Email))
                {
                    paciente.Email = paciente.Email;
                }
                else
                {
                    paciente.Email = Seguridad.Encrypt(paciente.Email);
                }

                //if (string.IsNullOrEmpty(paciente.Telefono))
                //{
                //    paciente.Telefono = paciente.Telefono;
                //}
                //else
                //{
                //    paciente.Telefono = Seguridad.Encrypt(paciente.Telefono);
                //}

                paciente.Telefono = paciente.Telefono;
                paciente.FechaNacimiento = paciente.FechaNacimiento;
                paciente.pacientes = contexto.pacientes.ToList();
                paciente.Sexo = paciente.Sexo;
                var p = contexto.pacientes.FirstOrDefault(model => model.Id == paciente.Id);
                try
                {
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
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage);
                        }
                    }
                }              
            }
            return RedirectToAction("index");
        }

        public string Texto(String texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                texto = string.Empty;
            }
            return texto; 
        }
    }
}