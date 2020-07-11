using System.ComponentModel.DataAnnotations;

namespace mvc.Models
{
    public class Conceptos
    {
        [Required]
        public int Id { set; get; }

        [Required]

        [Display(Name = "Clave")]
        public string Clave { set; get; }
        [Required]
        [Display(Name = "Concepto")]
        public string Concepto { set; get; }


    }
}