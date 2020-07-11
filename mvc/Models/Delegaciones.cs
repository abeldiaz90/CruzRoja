using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Delegaciones
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { set; get; }

        [Display(Name = "Estado:")]
        public IEnumerable<Estados> estado { set; get; }

        [ForeignKey("IdEstado")]
        public Estados estados { set; get; }

        [Display(Name = "Estado:")]
        [Required(ErrorMessage = "Seleccione un Estado")]
        public int IdEstado { set; get; }

        [Required(ErrorMessage = "Ingrese el nombre de la delegación")]
        [Display(Name = "Clave de delegación:")]
        public string claveDelegacion { set; get; }

        [Required(ErrorMessage = "Ingrese el Municipio")]
        [Display(Name = "Municipio:")]
        public string Municipio { set; get; }

        [Required(ErrorMessage = "Ingrese la colonia")]
        [Display(Name = "Colonia:")]
        public string Colonia { set; get; }

        [Required(ErrorMessage = "Ingrese la calle")]
        [Display(Name = "Calle:")]
        public string Calle { set; get; }

        [Required(ErrorMessage = "Ingrese el numero exterior")]
        [Display(Name = "Numero Exterior:")]
        public string NumeroExterior { set; get; }

        [Required(ErrorMessage = "Ingrese el numero interior")]
        [Display(Name = "Numero Interior:")]
        public string NumeroInterior { set; get; }

        [Required(ErrorMessage = "Ingrese el Codigo Postal")]
        [Display(Name = "Codigo Postal:")]
        public string CP { set; get; }

        [Required(ErrorMessage = "Ingrese el Telefono")]
        [Display(Name = "Telefono:")]
        public string Telefono { set; get; }
    }
}