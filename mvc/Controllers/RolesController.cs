using mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mvc.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        Contexto contexto = new Contexto();
        public ActionResult Index()
        {
            IEnumerable<Roles> roles = contexto.roles.ToList();
            foreach (var i in roles)
            {
                i.Rol = Seguridad.Decrypt(i.Rol);
            }
            return View(roles);
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        public ActionResult Editar(Roles roles)
        {
            Contexto con = new Contexto();
            var rol=con.roles.FirstOrDefault(x => x.Id == roles.Id);
            rol.Rol = Seguridad.Decrypt(rol.Rol);
            return View(rol);
        }

        public ActionResult Guardar(Roles roles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existe = contexto.roles.FirstOrDefault(x => x.Id == roles.Id);
                    roles.Rol = Seguridad.Encrypt(roles.Rol);
                    if (existe == null)
                    {
                        contexto.roles.Add(roles);
                        contexto.SaveChanges();
                    }
                    else
                    {
                        contexto.Set<Roles>().AddOrUpdate(roles);
                        contexto.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { ex.ToString(); }
            return RedirectToAction("Index");
        }
    }
}