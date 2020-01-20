using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc.Models;
namespace mvc.Controllers
{
    public class ServiciosPreciosController : Controller
    {
        Contexto con = new Contexto();
        // GET: ServiciosPrecios
        [HttpPost]
        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Index(int id)
        {
            ServiciosDelegacionPrecios sp = new ServiciosDelegacionPrecios();
            sp.serviciosDelegacionPrecios = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == id).OrderByDescending(s => s.PrecioSinIva);
            return PartialView("Index", sp.serviciosDelegacionPrecios);
            // return Json(sp.serviciosDelegacionPrecios,JsonRequestBehavior.AllowGet);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        // [HttpPost]
        public ActionResult Nuevo(int id)
        {
            ServiciosDelegacionPrecios sdp = new ServiciosDelegacionPrecios();
            IEnumerable<ServiciosDelegacion> serviciosDelegacions = con.serviciosdelegacion.ToList().OrderBy(s => s.NombreServicio);
            List<ServiciosDelegacion> listaservicios = new List<ServiciosDelegacion>();
            foreach (var i in serviciosDelegacions)
            {
                ServiciosDelegacion serviciosdelegaciones = new ServiciosDelegacion();
                serviciosdelegaciones.Id = i.Id;
                serviciosdelegaciones.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                listaservicios.Add(serviciosdelegaciones);
            }
            sdp.IdServicio = id;
            sdp.serviciosDelegacionPrecios = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == id).OrderByDescending(s => s.PrecioSinIva);
            sdp.listaserviciosdelegacion = listaservicios.OrderBy(s => s.NombreServicio);
            
            return View(sdp);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Editar(int id)
        {
            ServiciosDelegacionPrecios sdp = con.serviciosDelegacionPrecios.FirstOrDefault(s => s.Id == id);
            try
            {               
                IEnumerable<ServiciosDelegacion> serviciosDelegacions = con.serviciosdelegacion.ToList().OrderBy(s => s.NombreServicio);
                List<ServiciosDelegacion> listaservicios = new List<ServiciosDelegacion>();
                foreach (var i in serviciosDelegacions)
                {
                    ServiciosDelegacion serviciosdelegaciones = new ServiciosDelegacion();
                    serviciosdelegaciones.Id = i.Id;
                    serviciosdelegaciones.NombreServicio = Seguridad.Decrypt(i.NombreServicio);
                    listaservicios.Add(serviciosdelegaciones);
                }

                var idServicio = sdp.IdServicio;
                sdp.serviciosDelegacionPrecios = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == idServicio).OrderByDescending(s => s.PrecioSinIva); 
                sdp.listaserviciosdelegacion = listaservicios.OrderBy(s => s.NombreServicio);
            }
            catch (Exception Ex) { Ex.ToString(); }
            return View(sdp);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador, Capturista")]
        public ActionResult ConsultaPrecios(Int32 IdServicio)
        {
            ServiciosDelegacionPrecios sp = new ServiciosDelegacionPrecios();
            sp.serviciosDelegacionPrecios = con.serviciosDelegacionPrecios.ToList().Where(s => s.IdServicio == IdServicio).OrderByDescending(s=>s.PrecioSinIva);
            return Json(sp.serviciosDelegacionPrecios, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthFilter]
        [Authorize(Roles = "Administrador")]
        public ActionResult Guardar(ServiciosDelegacionPrecios serviciosDelegacionPrecios)
        {
            if (ModelState.IsValid)
            {
                // serviciosdelegacion.NombreServicio = Seguridad.Encrypt(serviciosdelegacion.NombreServicio);
                ServiciosDelegacionPrecios serviciosdelegacioneslistado = con.serviciosDelegacionPrecios.FirstOrDefault(model => model.Id == serviciosDelegacionPrecios.Id);
                if (serviciosdelegacioneslistado == null)
                {
                    con.serviciosDelegacionPrecios.Add(serviciosDelegacionPrecios);
                    con.SaveChanges();
                }
                else
                {
                    con.Set<ServiciosDelegacionPrecios>().AddOrUpdate(serviciosDelegacionPrecios);
                    con.SaveChanges();
                }

            }
            return RedirectToAction("Nuevo/"+ serviciosDelegacionPrecios.IdServicio);
        }
    }
}