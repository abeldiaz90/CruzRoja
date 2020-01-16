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
       // [Authorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            IEnumerable<Paises> paises = con.paises.ToList();
            IEnumerable<Estados> estados = con.estados.ToList();
            List<Paises> lp = new List<Paises>();
            List<Estados> le = new List<Estados>();
            foreach (var i in paises)
            {
                Paises p = new Paises();
                p.Id = i.Id;
                p.Pais = Seguridad.Decrypt(i.Pais);
                lp.Add(p);
            }

            foreach (var i in estados)
            {
                Estados e = new Estados();
                e.Id = i.Id;
                e.Estado = Seguridad.Decrypt(i.Estado);
                e.IdPais = i.IdPais;
                e.pais = i.pais;
                e.paises = i.paises;         
                le.Add(e);
            }

            var vistaestados = from p in lp
                               join e in le on p.Id equals e.IdPais
                               select new EstadosVista { paises = p, estados = e };
    

            return View(vistaestados.OrderBy(s=>s.estados.Estado));
        }
       // [Authorize(Roles = "Administrador")]
        public ActionResult Nuevo()
        {
            Estados estadoslistado = new Estados();
            estadoslistado.pais = con.paises.ToList();
            estadoslistado.Id = estadoslistado.Id;
            estadoslistado.IdPais = estadoslistado.IdPais;
            estadoslistado.Estado = Seguridad.Decrypt(estadoslistado.Estado);
            estadoslistado.pais = con.paises.ToList();
            List<Paises> paises = new List<Paises>();
            foreach (var i in estadoslistado.pais)
            {
                Paises p = new Paises();
                p.Id = i.Id;
                p.Pais = Seguridad.Decrypt(i.Pais);
                paises.Add(p);
            }
            estadoslistado.pais = paises;
            return View(estadoslistado);
        }

        [HttpGet]
       // [Authorize(Roles = "Administrador")]
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
       // [Authorize(Roles = "Administrador")]
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