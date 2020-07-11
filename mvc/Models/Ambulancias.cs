using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Ambulancias
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 IdAmbulancia { set; get; }
        [Required]
        [Display(Name = "Clave de Unidad:")]
        public String Unidad { set; get; }
    }
}