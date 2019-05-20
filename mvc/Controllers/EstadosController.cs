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
            var estadoslistado = con.estados.FirstOrDefault(model => model.Id == estados.Id);
            Estados estadoslista = new Estados();
            estadoslista.Id = estadoslistado.Id;
            estadoslista.IdPais = estadoslistado.IdPais;
            estadoslista.Estado = Seguridad.Decrypt(estadoslistado.Estado);
            estadoslista.pais = con.paises.ToList();
           
            List<Paises> paises = new List<Paises>();
            foreach (var i in estadoslista.pais)
            {
                Paises p = new Paises();
                p.Id = i.Id;
                p.Pais = Seguridad.Decrypt(i.Pais);
                paises.Add(p);
            }
            estadoslista.pais = paises;

            return View(estadoslista);
        }

        [HttpPost]
        public ActionResult Guardar(Estados estados)
        {
            if (ModelState.IsValid)
            {
                var estadoslistado = con.estados.FirstOrDefault(model => model.Id == estados.Id);
                if (estadoslistado == null)
                {
                    Estados estadoslista = new Estados();
                    estadoslista.Id = estados.Id;
                    estadoslista.IdPais = estados.IdPais;
                    estadoslista.pais = estados.pais;
                    estadoslista.Estado = Seguridad.Encrypt(estados.Estado);
                    con.estados.Add(estadoslista);
                    con.SaveChanges();
                }
                else
                {
                    Estados estadoslista = new Estados();
                    estadoslista.Id = estados.Id;
                    estadoslista.IdPais = estados.IdPais;
                    estadoslista.pais = estados.pais;
                    estadoslista.Estado = Seguridad.Encrypt(estados.Estado);

                    con.Set<Estados>().AddOrUpdate(estadoslista);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }
    }
}