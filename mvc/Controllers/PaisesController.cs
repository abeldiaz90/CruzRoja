using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
using mvc;
namespace mvc.Controllers
{
    public class PaisesController : Controller
    {
        // GET: Paises
        Contexto con = new Contexto();

       // [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Paises> paises = con.paises.ToList();
            List<Paises> listaPaises = new List<Paises>();
            
            foreach (var i in paises)
            {
                Paises p = new Paises();
                p.Id = i.Id;
                p.Pais = Seguridad.Decrypt(i.Pais);
                listaPaises.Add(p);
            }
            return View(listaPaises.OrderBy(s => s.Pais));
        }


      //  [Authorize(Roles = "Administrador")]
        public ActionResult Nuevo()
        {
            return View();            
        }

        [HttpGet]
       // [Authorize(Roles = "Administrador")]
        public ActionResult Editar(Paises paises)
        {            
            var paiseslistado = con.paises.FirstOrDefault(model => model.Id == paises.Id);
            Paises p = new Paises();
            p.Id = paiseslistado.Id;
            p.Pais = Seguridad.Decrypt(paiseslistado.Pais);
            return View(p);
        }

        [HttpPost]
       // [Authorize(Roles = "Administrador")]
        public ActionResult Guardar(Paises paises)
        {
            if (ModelState.IsValid)
            {
                Paises p = new Paises();
                p.Id = paises.Id;
                p.Pais = Seguridad.Encrypt(paises.Pais);

                Paises paiseslistado = con.paises.FirstOrDefault(model => model.Id == paises.Id);
                if (paiseslistado == null)
                {
                 
                    con.paises.Add(p);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<Paises>().AddOrUpdate(p);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }

    }
}