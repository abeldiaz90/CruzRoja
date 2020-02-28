using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace mvc.Controllers
{
    
    public class OrdenesController : Controller
    {
        // GET: Ordenes
        Contexto contexto = new Contexto();
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        //[Microsoft.AspNetCore.Authorization.Authorize(Policy = "Ventas")]     
        public ActionResult Index()
        {
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
            contexto.Dispose();
            return View(or);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
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
            contexto.Dispose();
            return l;
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public PartialViewResult Agregar(Ordenes ordenes)
        {
            Contexto con = new Contexto();
            Int32 itemAgregado = con.ordenestemporal.Count(X => X.IdServicio == ordenes.ordentemporal.IdServicio);
            if (itemAgregado == 0)
            {
                ordenes.ordentemporal.Agregado = true;
            }
            else { ordenes.ordentemporal.Agregado = false; }
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
            ordenes.ordentemporal.IdFolio = ordenes.Id;
            con.ordenestemporal.Add(ordenes.ordentemporal);
            con.SaveChanges();
            con.Dispose();
            IEnumerable<OrdenesTemporalVista> vistaestados = OrdenDetalle(ordenes.Id);
            return PartialView("OrdenesTemporal", vistaestados);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
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
            con.Dispose();
            return PartialView("OrdenesTemporal", vistaestados);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Editar(int Id)
        {
            Contexto con = new Contexto();
            OrdenesTemporal ordenesTemporal = con.ordenestemporal.FirstOrDefault(s => s.Id == Id);
            con.Dispose();
            return Json(ordenesTemporal, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
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
            con.Dispose();
            return PartialView("OrdenesTemporal", vistaestados);
        }


        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult Cobrar(Ordenes ordenes)
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
            ordenesmodelo.Factura = ordenes.Factura;
            var username = Seguridad.Encrypt(HttpContext.User.Identity.Name);
            int idenc = con.users.FirstOrDefault(x => x.Usuario == username).IdDelegacion;
            int idsuario=con.users.FirstOrDefault(x => x.Usuario == username).Id;
            ordenesmodelo.IdDelegacionExpedicion = idenc;
            ordenesmodelo.IdUsuario = idsuario;

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
                    if (ordenesmodelo.Factura)
                    {
                        ordenesDetalles.IVA = i.IVA;
                    }
                    else { ordenesDetalles.IVA = 0; }
                    ordenesDetalles.Total = i.Total;
                    con.ordenesdetalles.Add(ordenesDetalles);
                }
                con.ordenestemporal.RemoveRange(con.ordenestemporal.Where(x => x.IdFolio == ordenes.Id));
                con.SaveChanges();
                con.Dispose();
            }
            return Json(true,JsonRequestBehavior.AllowGet);
          //  return RedirectToAction("Index");
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public PartialViewResult Recibo(int numeroorden)
        {
            IEnumerable<Ordenes> orden = contexto.ordenes.ToList().Where(s => s.Id == numeroorden);
            IEnumerable<OrdenesDetalles> ordendetalles = contexto.ordenesdetalles.Where(m => m.IdFolio == numeroorden).ToList();
            IEnumerable<ServiciosDelegacion> serviciosDelegacion = contexto.serviciosdelegacion.ToList();
            int idpaciente = orden.FirstOrDefault().Idpaciente;
            IEnumerable<Pacientes> pacientes = contexto.pacientes.ToList().Where(x => x.Id == idpaciente);

            decimal rsubtotal = 0;
            decimal riva = 0;
            decimal rtotal = 0;
            foreach (var i in ordendetalles)
            {
                rsubtotal = rsubtotal + i.subtotal;
                riva = riva + i.IVA;
            }
            rtotal = rsubtotal + riva;

            foreach (var i in pacientes)
            {
                i.Nombre = Seguridad.Decrypt(i.Nombre);
                i.SegundoNombre = Seguridad.Decrypt(i.SegundoNombre);
                i.ApellidoPaterno = Seguridad.Decrypt(i.ApellidoPaterno);
                i.ApellidoMaterno = Seguridad.Decrypt(i.ApellidoMaterno);
            }

            var iddelegacion = 0;
            foreach (var i in serviciosDelegacion)
            {
                i.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                iddelegacion = i.IdDelegacion;
            }



            var delnombre = Seguridad.Decrypt(contexto.delegaciones.FirstOrDefault(x => x.Id == iddelegacion).Municipio);
            var vistaRecibo = from Ord in orden
                              join ord_det in ordendetalles on Ord.Id equals ord_det.IdFolio
                              join sd in serviciosDelegacion on ord_det.IdServicio equals sd.Id
                              join pa in pacientes on Ord.Idpaciente equals pa.Id
                              orderby sd.NombreServicio
                              select new OrdenesRecibos { ordenes = Ord, ordenesDetalles = ord_det, serviciosDelegacion = sd, pacientes = pa, subtotal = rsubtotal, total = rtotal, IVA = riva, totalpagado = Ord.Total, pagacon = Ord.PagaCon, cambio = Ord.cambio, Delegacion = delnombre, letras = enletras(Ord.Total) };

            return PartialView("Recibo", vistaRecibo);
        }

        public string enletras(decimal num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            decimal nro;

            try

            {
                nro = num;
            }
            catch
            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }

            res = toText(Convert.ToDecimal(entero)) + dec;
            return res;
        }

        private string toText(decimal value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }
    }
}
