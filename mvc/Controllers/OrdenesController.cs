using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using mvc.Models;

namespace mvc.Controllers
{
    public class OrdenesController : Controller
    {
        // GET: Ordenes
        public ActionResult Index()
        {
            Ordenes or = new Ordenes();
            Contexto contexto = new Contexto();
            int maxId = 0;
            try
            {
                maxId = (from c in contexto.ordenes select c.Id).Max();
                or.Id = maxId;
            }
            catch (Exception ex) { or.Id = 1; }

            IEnumerable<ServiciosDelegacion> serviciosdelegacion = contexto.serviciosdelegacion.ToList();
            IEnumerable<OrdenesTemporal> ordenestemporal = contexto.ordenestemporal.Where(m => m.IdFolio == or.Id).ToList();
            var vistaestados = from ot in ordenestemporal
                               join se in serviciosdelegacion on ot.IdServicio equals se.Id
                               select new OrdenesTemporalVista { ordenesTemporal = ot, serviciosDelegacion = se };

            or.pacientes = contexto.pacientes.ToList();
            or.serviciosDelegacions = contexto.serviciosdelegacion.ToList();
            or.ordenestemporalvista = vistaestados;
            return View(or);
        }

        public async Task <ActionResult> Agregar(Ordenes ordenes)
        {
            Contexto con = new Contexto();
            con.ordenestemporal.Add(ordenes.ordentemporal);
            con.SaveChanges();
            return RedirectToAction("Index");
        }
        public  ActionResult Eliminar(int Id)
        {
            Contexto con = new Contexto();
            OrdenesTemporal ot = con.ordenestemporal.FirstOrDefault(s => s.Id == Id);
            con.ordenestemporal.Remove(ot);
            con.SaveChanges();
            return RedirectToAction("Index");
        }


        // [HttpPost]
        /*  public ActionResult Editar(Ordenes ordenes)
          {
              Contexto con = new Contexto();
              if (ModelState.IsValid)
              {
                  OrdenesTemporal ordenesTemporal = con.ordenestemporal.FirstOrDefault(model => model.Id == ordenes.);
                  if (ordenesTemporal == null)
                  {
                      con.ordenestemporal.Add(ordenes.ordenestemporal);
                      con.SaveChanges();
                  }
                  else
                  {
                      con.Set<OrdenesTemporal>().AddOrUpdate(paises);
                      con.SaveChanges();
                  }

              }
              return RedirectToAction("Index");
          }*/

    }
}