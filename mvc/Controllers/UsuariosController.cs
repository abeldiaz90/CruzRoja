using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class UsuariosController : Controller
    {
        Contexto con = new Contexto();
        // GET: Usuarios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Nuevo()
        {
            Usuarios us = new Usuarios();
            us.delegaciones = con.delegaciones.ToList().OrderBy(s=>s.Municipio);
            return View(us);
        }



        [HttpPost]
        public ActionResult Guardar(Usuarios usuarios)
        {
            usuarios.Usuario = Seguridad.Encrypt(usuarios.Usuario);
            usuarios.Password = Seguridad.Encrypt(usuarios.Password);
            usuarios.Correo = Seguridad.Encrypt(usuarios.Correo);
            
            Usuarios us = con.usuarios.FirstOrDefault(s => s.Usuario == usuarios.Usuario && s.Password == usuarios.Password);

            if (us == null)
            {
                con.usuarios.Add(usuarios);
                con.SaveChanges();
            }
            else
            {
                con.Set<Usuarios>().AddOrUpdate(usuarios);
                con.SaveChanges();
            }
            return RedirectToAction("/Ordenes/Index");
        }
    }
}