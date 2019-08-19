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
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            IEnumerable<Users> usuarios = con.users.ToList();
            List<Users> listausarios = new List<Users>();
            foreach (var i in usuarios)
            {
                Users usuario = new Users();
                usuario.Id = i.Id;
                usuario.Usuario = Seguridad.Decrypt(i.Usuario);
                usuario.delegacion = i.delegacion;
                usuario.Correo = Seguridad.Decrypt(i.Correo);
                listausarios.Add(usuario);
            }
            return View(listausarios);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Nuevo()
        {
            Users us = new Users();
            IEnumerable<Roles> listaroles = con.roles.ToList();
            List<Roles> roles = new List<Roles>();

            IEnumerable<Delegaciones> delegaciones = con.delegaciones.ToList().OrderBy(s => s.Municipio);
            List<Delegaciones> listadelegaciones = new List<Delegaciones>();
            foreach (var i in listaroles)
            {
                Roles r = new Roles();
                r.Id = i.Id;
                r.Rol = Seguridad.Decrypt(i.Rol);
                roles.Add(r);
            }

            foreach (var delegacion in delegaciones)
            {
                Delegaciones d = new Delegaciones();
                d.Id = delegacion.Id;
                d.Municipio = Seguridad.Decrypt(delegacion.Municipio);
                listadelegaciones.Add(d);
            }

            us.roleslista = roles;
            us.delegaciones = listadelegaciones;
            return View(us);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Editar(int id)
        {
            Users us = new Users();
            IEnumerable<Roles> listaroles = con.roles.ToList().Where(x => x.Id == id);
            List<Roles> roles = new List<Roles>();

            IEnumerable<Delegaciones> delegaciones = con.delegaciones.ToList().OrderBy(s => s.Municipio);
            List<Delegaciones> listadelegaciones = new List<Delegaciones>();
            foreach (var i in listaroles)
            {
                Roles r = new Roles();
                r.Id = i.Id;
                r.Rol = Seguridad.Decrypt(i.Rol);
                roles.Add(r);
            }

            foreach (var delegacion in delegaciones)
            {
                Delegaciones d = new Delegaciones();
                d.Id = delegacion.Id;
                d.Municipio = Seguridad.Decrypt(delegacion.Municipio);
                listadelegaciones.Add(d);
            }

            us.roleslista = roles;
            us.delegaciones = listadelegaciones;
            return View(us);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Guardar(Users usuarios)
        {
            usuarios.Usuario = Seguridad.Encrypt(usuarios.Usuario);
            usuarios.Password = Seguridad.Encrypt(usuarios.Password);
            usuarios.Correo = Seguridad.Encrypt(usuarios.Correo);

            Users us = con.users.FirstOrDefault(s => s.Usuario == usuarios.Usuario && s.Password == usuarios.Password);

            if (us == null)
            {
                con.users.Add(usuarios);
                con.SaveChanges();
            }
            else
            {
                con.Set<Users>().AddOrUpdate(usuarios);
                con.SaveChanges();
            }
            return RedirectToAction("/Ordenes/Index");
        }
    }
}