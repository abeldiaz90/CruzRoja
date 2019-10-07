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

        Contexto contexto = new Contexto();
        [Authorize(Roles = "secretaria")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("~/Account/Index");
            }
            Ordenes or = new Ordenes();
            int maxId = 0;
            try
            {
                maxId = (from c in contexto.ordenes select c.Id).Max();
                or.Id = maxId + 1;
            }
            catch (Exception ex) { or.Id = 1; }

            IEnumerable<ServiciosDelegacion> sd = contexto.serviciosdelegacion.ToList().OrderBy(s => s.NombreServicio);
            List<ServiciosDelegacion> serviciosDelegacions = new List<ServiciosDelegacion>();
            foreach (var i in sd)
            {
                ServiciosDelegacion s = new ServiciosDelegacion();
                s.Id = i.Id;
                s.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                serviciosDelegacions.Add(s);
            }
            or.pacientes = contexto.pacientes.ToList();
            or.serviciosDelegacionPrecios = contexto.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == 0);
            or.serviciosDelegacions = serviciosDelegacions.OrderBy(s => s.NombreServicio);
            or.ordenestemporalvista = OrdenDetalle(or.Id);
            return View(or);
        }

        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "secretaria")]
        public IEnumerable<OrdenesTemporalVista> OrdenDetalle(Int32 Id)
        {
            IEnumerable<ServiciosDelegacion> serviciosdelegacion = contexto.serviciosdelegacion.ToList();
            IEnumerable<OrdenesTemporal> ordenestemporal = contexto.ordenestemporal.Where(m => m.IdFolio == Id).ToList();
            IEnumerable<ServiciosDelegacionPrecios> serviciosDelegacionPrecios = contexto.serviciosDelegacionPrecios.ToList();
            var vistaestados = from ot in ordenestemporal
                               join se in serviciosdelegacion on ot.IdServicio equals se.Id
                               join pr in serviciosDelegacionPrecios on ot.IdPrecio equals pr.Id
                               orderby serviciosdelegacion.First()
                               select new OrdenesTemporalVista { ordenesTemporal = ot, serviciosDelegacion = se, ServiciosDelegacionPrecios = pr };
            List<OrdenesTemporalVista> l = new List<OrdenesTemporalVista>();

            decimal Total = 0;
            foreach (var i in vistaestados)
            {
                OrdenesTemporalVista otv = new OrdenesTemporalVista();
                if (i.serviciosDelegacion.AplicaIVA)
                {
                    decimal subtotal = (i.ordenesTemporal.cantidad) * (i.ServiciosDelegacionPrecios.PrecioSinIva);
                    i.ordenesTemporal.subtotal = subtotal;
                    i.ordenesTemporal.IVA = ((subtotal) * decimal.Parse(("0.16")));
                    Total = ((subtotal) * decimal.Parse(("0.16"))) + subtotal;
                    i.ordenesTemporal.Total = Total;
                }
                else
                {
                    decimal subtotal = (i.ordenesTemporal.cantidad) * (i.ServiciosDelegacionPrecios.PrecioSinIva);
                    i.ordenesTemporal.subtotal = subtotal;
                    i.ordenesTemporal.IVA = 0;
                    Total = subtotal;
                    i.ordenesTemporal.Total = Total;
                }
                otv = i;
                l.Add(otv);
            }

            return l;
        }

        [Authorize(Roles = "secretaria")]
        [Authorize(Roles = "Admin")]
        public PartialViewResult Agregar(Ordenes ordenes)
        {
            Contexto con = new Contexto();
            var Iva = con.serviciosdelegacion.Where(x => x.Id == ordenes.ordentemporal.IdServicio).FirstOrDefault().AplicaIVA;

            if (Iva)
            {
                ordenes.ordentemporal.IVA = ((ordenes.ordentemporal.cantidad) * con.serviciosDelegacionPrecios.Where(x => x.Id == ordenes.ordentemporal.IdPrecio).FirstOrDefault().PrecioSinIva) * decimal.Parse("0.16");
            }
            else
            {
                ordenes.ordentemporal.IVA = 0;
            }
            ordenes.ordentemporal.subtotal = (ordenes.ordentemporal.cantidad) * con.serviciosDelegacionPrecios.Where(x => x.Id == ordenes.ordentemporal.IdPrecio).FirstOrDefault().PrecioSinIva;
            ordenes.ordentemporal.Total = ordenes.ordentemporal.IVA + ordenes.ordentemporal.subtotal;
            //ordenes.TotalIVA = con.ordenestemporal.Where(x => x.Id == ordenes.ordentemporal.Id).Sum(x => x.IVA) + ordenes.ordentemporal.IVA;

            ordenes.ordentemporal.IdFolio = ordenes.Id;
            con.ordenestemporal.Add(ordenes.ordentemporal);
            con.SaveChanges();
            IEnumerable<OrdenesTemporalVista> vistaestados = OrdenDetalle(ordenes.Id);
            return PartialView("OrdenesTemporal", vistaestados);
        }

        [Authorize(Roles = "secretaria")]
        [Authorize(Roles = "Admin")]
        public PartialViewResult Eliminar(int Id, int IdFolio)
        {
            Contexto con = new Contexto();
            OrdenesTemporal ordenesTemporal = con.ordenestemporal.FirstOrDefault(s => s.Id == Id);
            con.ordenestemporal.Remove(ordenesTemporal);
            con.SaveChanges();

            IEnumerable<ServiciosDelegacion> serviciosdelegacion = con.serviciosdelegacion.ToList();
            IEnumerable<OrdenesTemporal> ordenestemporal = con.ordenestemporal.Where(m => m.IdFolio == IdFolio).ToList();
            IEnumerable<ServiciosDelegacionPrecios> serviciosDelegacionPrecios = con.serviciosDelegacionPrecios.ToList();
            IEnumerable<OrdenesTemporalVista> vistaestados = OrdenDetalle(IdFolio);
            return PartialView("OrdenesTemporal", vistaestados);
        }

        [Authorize(Roles = "secretaria")]
        [Authorize(Roles = "Admin")]
        public ActionResult Editar(int Id)
        {
            Contexto con = new Contexto();
            OrdenesTemporal ordenesTemporal = con.ordenestemporal.FirstOrDefault(s => s.Id == Id);
            return Json(ordenesTemporal, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "secretaria")]
        [Authorize(Roles = "Admin")]
        public PartialViewResult Cancelar(int Id)
        {
            Contexto con = new Contexto();
            List<OrdenesTemporal> ordenesTemporal = con.ordenestemporal.Where(s => s.IdFolio == Id).ToList();
            con.ordenestemporal.RemoveRange(ordenesTemporal);
            con.SaveChanges();

            IEnumerable<ServiciosDelegacion> serviciosdelegacion = con.serviciosdelegacion.ToList();
            IEnumerable<OrdenesTemporal> ordenestemporal = con.ordenestemporal.Where(m => m.IdFolio == Id).ToList();
            IEnumerable<ServiciosDelegacionPrecios> serviciosDelegacionPrecios = con.serviciosDelegacionPrecios.ToList();
            IEnumerable<OrdenesTemporalVista> vistaestados = OrdenDetalle(Id);
            return PartialView("OrdenesTemporal", vistaestados);
        }


        [Authorize(Roles = "secretaria")]
        [Authorize(Roles = "Admin")]
        public PartialViewResult Cobrar(Ordenes ordenes)
        {
            Contexto con = new Contexto();
            Ordenes ordenesmodelo = new Ordenes();
            ordenesmodelo.Total = ordenes.Total;
            ordenesmodelo.cambio = ordenes.cambio;
            ordenesmodelo.FechaHora = System.DateTime.Now;
            ordenesmodelo.PagaCon = ordenes.PagaCon;
            ordenesmodelo.Id = ordenes.Id;
            ordenesmodelo.Idpaciente = ordenes.Idpaciente;
            ordenesmodelo.formapago = ordenes.formapago;

            var validar = con.ordenes.FirstOrDefault(m => m.Id == ordenes.Id);
            if (validar == null)
            {
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

                foreach (var i in ordenesTemporals)
                {
                    OrdenesDetalles ordenesDetalles = new OrdenesDetalles();
                    ordenesDetalles.IdFolio = i.IdFolio;
                    ordenesDetalles.IdServicio = i.IdServicio;
                    ordenesDetalles.cantidad = i.cantidad;
                    ordenesDetalles.Precio = contexto.serviciosDelegacionPrecios.Where(x => x.Id == i.IdPrecio).FirstOrDefault().PrecioSinIva;
                    ordenesDetalles.subtotal = i.subtotal;
                    ordenesDetalles.IVA = i.IVA;
                    ordenesDetalles.Total = i.Total;
                    con.ordenesdetalles.Add(ordenesDetalles);
                }
                con.ordenestemporal.RemoveRange(con.ordenestemporal.Where(x => x.IdFolio == ordenes.Id));
                con.SaveChanges();

            }
            IEnumerable<Ordenes> orden = contexto.ordenes.ToList().Where(s => s.Id == ordenes.Id);
            IEnumerable<OrdenesDetalles> ordendetalles = contexto.ordenesdetalles.Where(m => m.IdFolio == ordenes.Id).ToList();
            IEnumerable<ServiciosDelegacion> serviciosDelegacion = contexto.serviciosdelegacion.ToList();
            IEnumerable<Pacientes> pacientes = contexto.pacientes.ToList().Where(x => x.Id == ordenes.Idpaciente);

            var vistaRecibo = from Ord in orden
                              join ord_det in ordendetalles on Ord.Id equals ord_det.IdFolio
                              join sd in serviciosDelegacion on ord_det.IdServicio equals sd.Id
                              join pa in pacientes on Ord.Idpaciente equals pa.Id
                              orderby sd.NombreServicio
                              select new OrdenesRecibos { ordenes = Ord, ordenesDetalles = ord_det, serviciosDelegacion = sd, pacientes = pa };

            foreach (var i in vistaRecibo)
            {
                i.pacientes.Nombre = Seguridad.Decrypt(i.pacientes.Nombre);
                i.pacientes.SegundoNombre = Seguridad.Decrypt(i.pacientes.SegundoNombre);
                i.pacientes.ApellidoPaterno = Seguridad.Decrypt(i.pacientes.ApellidoPaterno);
                i.pacientes.ApellidoMaterno = Seguridad.Decrypt(i.pacientes.ApellidoMaterno);
                i.pacientes.FechaNacimiento = i.pacientes.FechaNacimiento;
                i.pacientes.Sexo = i.pacientes.Sexo;
                i.ordenesDetalles.cantidad = i.ordenesDetalles.cantidad;
                i.ordenesDetalles.Precio = i.ordenesDetalles.Precio;
                i.ordenesDetalles.subtotal = i.ordenesDetalles.subtotal;
                i.ordenesDetalles.IVA = i.ordenesDetalles.IVA;
                i.ordenesDetalles.Total = i.ordenesDetalles.Total;
                i.serviciosDelegacion.NombreServicio = Seguridad.Decrypt(i.serviciosDelegacion.NombreServicio);
                i.ordenes.PagaCon = i.ordenes.PagaCon;
                i.ordenes.cambio = i.ordenes.cambio;
            }
            return PartialView("Recibo", vistaRecibo);
        }

    }
}
