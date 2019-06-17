using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class AccountController : Controller
    {
        Contexto con = new Contexto();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //[Authorize]
        public ActionResult Login(Usuarios usuarios)
        {
            if (string.IsNullOrWhiteSpace(usuarios.Usuario))
            {
                ModelState.AddModelError("", "Búsqueda inválida.");
                return View(usuarios);
            }
            usuarios.Usuario = Seguridad.Encrypt(usuarios.Usuario);
            usuarios.Password = Seguridad.Encrypt(usuarios.Password);
            Usuarios us = con.usuarios.FirstOrDefault(s => s.Usuario == usuarios.Usuario && s.Password == usuarios.Password);
            String Resultado = "";

            if (us == null)
            {
                Resultado = "Fallido";
            }
            else { Resultado = "Exitoso"; }
            //return RedirectToAction("Index", "Account");

            return Json(Resultado, JsonRequestBehavior.AllowGet);
        }
    }
}