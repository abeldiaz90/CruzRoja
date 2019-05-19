using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class ServiciosController : Controller
    {
        Contexto con = new Contexto();
        // GET: Servicios
        public ActionResult Index()
        {          
            IEnumerable<Servicios> servicios = con.servicios.ToList();
            return View(servicios);
        }

        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Editar(Servicios servicios)
        {
            Servicios ser = con.servicios.FirstOrDefault(model => model.Id == servicios.Id);
            return View(ser);
        }

        [HttpPost]
        public ActionResult Guardar(Servicios servicios)
        {
            if (ModelState.IsValid)
            {
                Servicios ser = con.servicios.FirstOrDefault(model => model.Id == servicios.Id);
                if (ser == null)
                {
                    con.servicios.Add(servicios);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<Servicios>().AddOrUpdate(servicios);
                    con.SaveChanges();
                }
               
            }
            return RedirectToAction("Index");
        }
    }
}