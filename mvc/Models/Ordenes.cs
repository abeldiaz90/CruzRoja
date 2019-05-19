using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Ordenes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Display(Name = "Paciente:")]
        public int Idpaciente { set; get; }
        [Required(ErrorMessage ="Indique los datos del paciente")]
        public Pacientes Paciente { set; get; }
        [Display(Name = "Servicio:")]
        [Required(ErrorMessage ="Seleccione una opcion:")]
        public int IdServicio { set; get; }
        [Required(ErrorMessage ="Cantidad:")]
        [Display(Name ="Cantidad:")]
        public int Cantidad { set; get; }

        public OrdenesTemporal ordentemporal { set; get; }
        public IEnumerable<ServiciosDelegacion> serviciosDelegacions { set; get; }
        public IEnumerable<Pacientes> pacientes { set; get; }
        public IEnumerable<OrdenesTemporal> ordenestemporal { set; get; }
        public IEnumerable<OrdenesTemporalVista> ordenestemporalvista { set; get; }
    }
}