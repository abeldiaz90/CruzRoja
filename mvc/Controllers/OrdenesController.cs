using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
                or.Id = maxId + 1;
            }
            catch (Exception ex) { or.Id = 1; }

            IEnumerable<ServiciosDelegacion> serviciosdelegacion = contexto.serviciosdelegacion.ToList();
            IEnumerable<OrdenesTemporal> ordenestemporal = contexto.ordenestemporal.Where(m => m.IdFolio == or.Id).ToList();
            var vistaestados = from ot in ordenestemporal
                               join se in serviciosdelegacion on ot.IdServicio equals se.Id
                               orderby serviciosdelegacion.First()
                               select new OrdenesTemporalVista { ordenesTemporal = ot, serviciosDelegacion = se };


            or.pacientes = contexto.pacientes.ToList();
            or.serviciosDelegacions = contexto.serviciosdelegacion.ToList().OrderBy(s => s.NombreServicio);
            or.ordenestemporalvista = vistaestados.OrderBy(s => s.serviciosDelegacion.NombreServicio);
            return View(or);
        }


        [HttpPost]
        public ActionResult Agregar(Ordenes ordenes)
        {
            Contexto con = new Contexto();
            con.ordenestemporal.Add(ordenes.ordentemporal);
            con.SaveChanges();

            IEnumerable<ServiciosDelegacion> serviciosdelegacion = con.serviciosdelegacion.ToList();
            IEnumerable<OrdenesTemporal> ordenestemporal = con.ordenestemporal.Where(m => m.IdFolio == ordenes.ordentemporal.IdFolio).ToList();
            var vistaestados = from ot in ordenestemporal
                               join se in serviciosdelegacion on ot.IdServicio equals se.Id
                               orderby serviciosdelegacion.First()
                               select new OrdenesTemporalVista { ordenesTemporal = ot, serviciosDelegacion = se };

            ordenes.ordenestemporalvista = vistaestados.OrderBy(s => s.serviciosDelegacion.NombreServicio).OrderBy(s=>s.serviciosDelegacion.NombreServicio);
            return PartialView("OrdenesTemporal", ordenes.ordenestemporalvista);
        }
        public async Task<ActionResult> Eliminar(int Id)
        {
            Contexto con = new Contexto();
            OrdenesTemporal ot = con.ordenestemporal.FirstOrDefault(s => s.Id == Id);
            con.ordenestemporal.Remove(ot);
            con.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> Cobrar(Ordenes ordenes)
        {
            Contexto con = new Contexto();
            Ordenes ordenesmodelo = new Ordenes();
            ordenesmodelo.FechaHora = System.DateTime.Now;
            var validar = con.ordenes.FirstOrDefault(m => m.Id == ordenes.Id);
            if (validar == null)
            {
                var validarcliente = con.pacientes.FirstOrDefault(m => m.Nombre == ordenes.Paciente.Nombre & m.SegundoNombre == ordenes.Paciente.SegundoNombre & m.ApellidoPaterno == ordenes.Paciente.ApellidoPaterno & m.ApellidoMaterno == ordenes.Paciente.ApellidoMaterno);
                if (validarcliente == null)
                {
                    con.pacientes.Add(ordenes.Paciente);
                    con.SaveChanges();
                    ordenesmodelo.Idpaciente = con.pacientes.FirstOrDefault(m => m.Nombre == ordenes.Paciente.Nombre & m.SegundoNombre == ordenes.Paciente.SegundoNombre & m.ApellidoPaterno == ordenes.Paciente.ApellidoPaterno & m.ApellidoMaterno == ordenes.Paciente.ApellidoMaterno).Id;                 
                }
                else
                {
                    ordenesmodelo.Idpaciente = validarcliente.Id;
                }

                int maxId = 0;
                try
                {
                    maxId = (from c in con.ordenes select c.Id).Max();
                    ordenesmodelo.Id = maxId;
                }
                catch (Exception ex) { ordenesmodelo.Id = 1; }

                con.ordenes.Add(ordenesmodelo);
                con.SaveChanges();


                IEnumerable<OrdenesTemporal> ordenesTemporals = con.ordenestemporal.Where(s => s.IdFolio == ordenes.Id);
                //List<OrdenesDetalles> ordenesDetallesListado = new List<OrdenesDetalles>();

                foreach (var i in ordenesTemporals)
                {
                    OrdenesDetalles ordenesDetalles = new OrdenesDetalles();
                    ordenesDetalles.IdFolio = i.IdFolio;
                    ordenesDetalles.IdServicio = i.IdServicio;
                    ordenesDetalles.cantidad = i.cantidad;
                    con.ordenesdetalles.Add(ordenesDetalles);
                }

            
                con.ordenestemporal.RemoveRange(con.ordenestemporal.Where(x => x.IdFolio == ordenes.Id));
                con.SaveChanges();

            }

            return RedirectToAction("Index");
        }
    }
}