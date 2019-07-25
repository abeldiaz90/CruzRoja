using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class OrdenesRecibos
    {
        public Ordenes ordenes { set; get; }
        public OrdenesDetalles ordenesDetalles { set; get; }
        public ServiciosDelegacion serviciosDelegacion { set; get; }
        public Pacientes pacientes { set; get; }
    }
}