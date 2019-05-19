using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class PaisesController : Controller
    {
        // GET: Paises
        Contexto con = new Contexto();
        public ActionResult Index()
        {
            IEnumerable<Paises> paises = con.paises.ToList();
            return View(paises);
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Editar(Paises paises)
        {
            Paises paiseslistado = con.paises.FirstOrDefault(model => model.Id == paises.Id);
            return View(paiseslistado);
        }

        [HttpPost]
        public ActionResult Guardar(Paises paises)
        {
            if (ModelState.IsValid)
            {
                Paises paiseslistado = con.paises.FirstOrDefault(model => model.Id == paises.Id);
                if (paiseslistado == null)
                {
                    con.paises.Add(paises);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<Paises>().AddOrUpdate(paises);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }

    }
}