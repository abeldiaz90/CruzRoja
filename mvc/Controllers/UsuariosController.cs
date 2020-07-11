using mvc.Models;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
namespace mvc.Controllers
{
    public class UsuariosController : Controller
    {
        Contexto con = new Contexto();
        // GET: Usuarios
        //[CustomAuthFilter]
        //[Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Users> listausarios = con.users.ToList();
            IEnumerable<Roles> listaroles = con.roles.ToList();
            IEnumerable<Delegaciones> listadelegaciones = con.delegaciones.ToList();
            var ur = from us in listausarios
                     join lr in listaroles on us.IdRol equals lr.Id
                     join ld in listadelegaciones on us.IdDelegacion equals ld.Id
                     select new UsersRoles { roles = lr, users = us, delegaciones = ld };
            // IEnumerable<UsersRoles> listarolesusuarios = new IEnumerable<UsersRoles>;
            foreach (var i in ur)
            {
                // UsersRoles urlista = new UsersRoles();
                i.users.Id = i.users.Id;
                i.users.Usuario = Seguridad.Decrypt(i.users.Usuario);
                i.users.delegacion = i.users.delegacion;
                i.users.Correo = Seguridad.Decrypt(i.users.Correo);
                i.roles.Rol = Seguridad.Decrypt(i.roles.Rol);
                i.delegaciones.Municipio = Seguridad.Decrypt(i.delegaciones.Municipio);
                // ur.Add(urlista);
            }
            return View(listausarios);
        }

        //[CustomAuthFilter]
        //[Authorize(Roles = "Administrador")]
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

        //[CustomAuthFilter]
        //[Authorize(Roles = "Administrador")]
        public ActionResult Editar(int id)
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

            Contexto co = new Contexto();
            var usuarios = co.users.FirstOrDefault(x => x.Id == id);

            us.roleslista = roles;
            us.delegaciones = listadelegaciones;
            us.Id = usuarios.Id;
            us.IdDelegacion = usuarios.IdDelegacion;
            us.IdRol = usuarios.IdRol;
            us.Correo = Seguridad.Decrypt(usuarios.Correo);
            us.Usuario = Seguridad.Decrypt(usuarios.Usuario);

            return View(us);
        }

        [HttpPost]
        //[CustomAuthFilter]
        //[Authorize(Roles = "Administrador")]
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