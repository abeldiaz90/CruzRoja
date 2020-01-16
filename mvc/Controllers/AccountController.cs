using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using mvc.Models;
using Newtonsoft.Json;

namespace mvc.Controllers
{
    public class AccountController : Controller
    {
        Contexto con = new Contexto();
        // GET: Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Users usuarios)
        {
            if (string.IsNullOrWhiteSpace(usuarios.Usuario))
            {
                ModelState.AddModelError("", "Búsqueda inválida.");
                return View(usuarios);
            }
            usuarios.Usuario = Seguridad.Encrypt(usuarios.Usuario);
            usuarios.Password = Seguridad.Encrypt(usuarios.Password);

            Users us = con.users.FirstOrDefault(s => s.Usuario == usuarios.Usuario && s.Password == usuarios.Password);
           
            Boolean Resultado;

            if (us == null)
            {
                Resultado = false;
            }
            else
            {
                var rol = con.roles.FirstOrDefault(x => x.Id == us.IdRol).Rol;
                var authTicket = new FormsAuthenticationTicket(1,Seguridad.Decrypt(usuarios.Usuario),DateTime.Now, DateTime.Now.AddMinutes(20),false, Seguridad.Decrypt(rol));
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                Resultado = true;
            }
            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }

       // [Authorize(Roles = "secretaria")]
       // [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> signout() 
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }
    }


}