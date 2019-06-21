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
        public async Task<ActionResult> Login(Usuarios usuarios)
        {
            if (string.IsNullOrWhiteSpace(usuarios.Usuario))
            {
                ModelState.AddModelError("", "Búsqueda inválida.");
                return View(usuarios);
            }
            usuarios.Usuario = Seguridad.Encrypt(usuarios.Usuario);
            usuarios.Password = Seguridad.Encrypt(usuarios.Password);

            Usuarios us = con.usuarios.FirstOrDefault(s => s.Usuario == usuarios.Usuario && s.Password == usuarios.Password);
            Boolean Resultado;

            if (us == null)
            {
                Resultado = false;
            }
            else
            {
                FormsAuthentication.SetAuthCookie(usuarios.Usuario, false);
                // var login = HttpContext.(CookieAuthenticationDefaults.CookiePrefix, principal);
                Resultado = true;
            }
            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }
    }


}