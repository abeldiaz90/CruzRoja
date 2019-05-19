using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class EstadosController : Controller
    {
        // GET: Estados
        // GET: Paises
        Contexto con = new Contexto();
        public ActionResult Index()
        {
            IEnumerable<Paises> paises = con.paises.ToList();
            IEnumerable<Estados> estados = con.estados.ToList();
            var vistaestados = from p in paises
                              join e in estados on p.Id equals e.IdPais
                              select new EstadosVista { paises = p, estados = e };
            return View(vistaestados);
        }

        public ActionResult Nuevo()
        {
            Estados estadoslistado = new Estados();
            estadoslistado.pais = con.paises.ToList();
            return View(estadoslistado);
        }

        [HttpGet]
        public ActionResult Editar(Estados estados)
        {
            Estados estadoslistado = con.estados.FirstOrDefault(model => model.Id == estados.Id);
            estadoslistado.pais = con.paises.ToList();
            return View(estadoslistado);
        }

        [HttpPost]
        public ActionResult Guardar(Estados estados)
        {
            if (ModelState.IsValid)
            {
                Estados estadoslistado = con.estados.FirstOrDefault(model => model.Id == estados.Id);
                if (estadoslistado == null)
                {
                    con.estados.Add(estados);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<Estados>().AddOrUpdate(estados);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}