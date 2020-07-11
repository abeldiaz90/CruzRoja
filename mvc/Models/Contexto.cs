using System.Data.Entity;

namespace mvc.Models
{
    public class Contexto : DbContext
    {
        public Contexto() : base(System.Configuration.ConfigurationManager.ConnectionStrings["Conexion"].ToString())
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
        public DbSet<OrdenesDetalles> ordenesdetalles { set; get; }
        public DbSet<Users> users { set; get; }
        public DbSet<Roles> roles { set; get; }
        public DbSet<ServiciosDelegacionPrecios> serviciosDelegacionPrecios { set; get; }

        public System.Data.Entity.DbSet<mvc.Models.Ambulancias> Ambulancias { get; set; }

        public System.Data.Entity.DbSet<mvc.Models.Claves> Claves { get; set; }

        public System.Data.Entity.DbSet<mvc.Models.Colonias> Colonias { get; set; }

        public System.Data.Entity.DbSet<mvc.Models.Traslados> Traslados { get; set; }

        public System.Data.Entity.DbSet<mvc.Models.ServiciosAmbulancias> ServiciosAmbulancias { get; set; }
    }
}