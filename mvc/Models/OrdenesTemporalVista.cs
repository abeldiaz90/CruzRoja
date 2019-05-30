using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class OrdenesTemporalVista
    {
        public ServiciosDelegacion serviciosDelegacion { set; get; }
        public OrdenesTemporal ordenesTemporal { set; get; }
        public ServiciosDelegacionPrecios ServiciosDelegacionPrecios { set; get; }
    }
}