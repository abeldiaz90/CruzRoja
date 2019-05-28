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
        [Key]
        public int Id { set; get; }

        [Display(Name = "Paciente:")]
        [Required(ErrorMessage = "Indique los datos del paciente")]
        public int Idpaciente { set; get; }
      
        public Pacientes Paciente { set; get; }


        [Display(Name ="Fecha y Hora:")]  
        [DataType(DataType.DateTime)]
        public DateTime FechaHora { set; get; }    

        public OrdenesTemporal ordentemporal { set; get; }
        public IEnumerable<ServiciosDelegacion> serviciosDelegacions { set; get; }
        public IEnumerable<Pacientes> pacientes { set; get; }
        public IEnumerable<OrdenesTemporal> ordenestemporal { set; get; }
        public IEnumerable<OrdenesTemporalVista> ordenestemporalvista { set; get; }
        public IEnumerable<ServiciosDelegacionPrecios> serviciosDelegacionPrecios { set; get; }
    }
}