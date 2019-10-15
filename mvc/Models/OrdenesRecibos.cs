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
        public DelegacionesVista delegacionesVista { set; get; }
        public decimal subtotal { set; get; }
        public decimal IVA { set; get; }
        public decimal total { set; get; }
        public decimal pagacon { set; get; }
        public decimal cambio { set; get; }
        public decimal totalpagado { set; get; }
        public string Delegacion { set; get; }
        public string letras { set; get; }

    }
}