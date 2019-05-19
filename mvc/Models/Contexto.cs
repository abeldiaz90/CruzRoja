using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Contexto:DbContext
    {
        public Contexto():base(System.Configuration.ConfigurationManager.ConnectionStrings["Conexion"].ToString())
        {
        }

        public DbSet<Paises> paises { set; get; }
        public DbSet<Estados> estados { set; get; }
        public DbSet<Delegaciones> delegaciones { set; get; }
        public DbSet<Servicios> servicios { set; get; }
        public DbSet<Pacientes> pacientes { set; get; }
        public DbSet<ServiciosDelegacion> serviciosdelegacion { set; get; }
        public DbSet<Ordenes> ordenes { set; get; }
        public DbSet<OrdenesTemporal> ordenestemporal { set; get; }

    }
}