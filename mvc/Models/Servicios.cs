using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Servicios
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { set; get; }
        [Required]
        [Display(Name = "Clave del Servicio:")]
        public string Clave { set; get; }

        [Required(ErrorMessage = "Indique el Nombre del servicio")]
        [Display(Name = "Nombre del Servicio:")]
        public string NombreServicio { set; get; }
    }
}