using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Conceptos
    {
        [Required]
        public int Id { set; get; }

        [Required]

        [Display(Name = "Clave")]
        public string Clave{set; get;}
        [Required]
        [Display(Name="Concepto")]
        public string Concepto { set; get; }

 
    }
}