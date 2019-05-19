using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class DelegacionesController : Controller
    {
        // GET: Estados
        // GET: Paises
        Contexto con = new Contexto();
        public ActionResult Index()
        {
            IEnumerable<Estados> estados = con.estados.ToList();
            IEnumerable<Delegaciones> delegaciones = con.delegaciones.ToList();
            var d = from del in delegaciones
                    join est in estados on del.IdEstado equals est.Id
                    select new DelegacionesVista { estados = est, delegaciones = del };
            return View(d);
        }

        public ActionResult Nuevo()
        {
            Delegaciones delegacioneslistado = new Delegaciones();
            delegacioneslistado.estado = con.estados.ToList();
            return View(delegacioneslistado);
        }

        [HttpGet]
        public ActionResult Editar(Delegaciones delegaciones)
        {
            Delegaciones delegacioneslistado = con.delegaciones.FirstOrDefault(model => model.Id == delegaciones.Id);
            delegacioneslistado.estado = con.estados.ToList();
            return View(delegacioneslistado);
        }

        [HttpPost]
        public ActionResult Guardar(Delegaciones delegaciones)
        {
            if (ModelState.IsValid)
            {
                Delegaciones delegacioneslistado = con.delegaciones.FirstOrDefault(model => model.Id == delegaciones.Id);
                if (delegacioneslistado == null)
                {
                    con.delegaciones.Add(delegaciones);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<Delegaciones>().AddOrUpdate(delegaciones);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}