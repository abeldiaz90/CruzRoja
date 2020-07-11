using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Claves
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 IdClave { set; get; }

        [Required]
        [Display(Name = "Clave de Emergencia:")]
        public String Clave { set; get; }

        [Required]
        [Display(Name = "Emergencia:")]
        public String Descripcion { set; get; }
    }

}