using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class ServiciosDelegacionPrecios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required(ErrorMessage = "Seleccione el Servicio:")]
        [Display(Name = "Servicio:")]
        public int IdServicio { set; get; }

        [Required(ErrorMessage = "Indique el precio sin Iva:")]
        [Display(Name = "Indique el precio sin Iva:")]
        public decimal PrecioSinIva { set; get; }
        public IEnumerable<ServiciosDelegacion> listaserviciosdelegacion { set; get; }
        public IEnumerable<ServiciosDelegacionPrecios> serviciosDelegacionPrecios { set; get; }


    }
}