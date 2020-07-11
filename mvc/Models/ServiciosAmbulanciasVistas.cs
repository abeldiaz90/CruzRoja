using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class ServiciosAmbulanciasVistas
    {
        public ServiciosAmbulancias serviciosAmbulancias { set; get; }
        public Colonias colonias { set; get; }
        public Claves claves { set; get; }
        public Ambulancias ambulancias { set; get; }
    }
}