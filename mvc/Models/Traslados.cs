using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Traslados
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 IdTraslado { set; get; }

        [Required]
        [Display(Name = "Clave:")]
        public String Clave { set; get; }
        [Required]
        [Display(Name = "Descripción:")]
        public String Descripcion { set; get; }
    }
}