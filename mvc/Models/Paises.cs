using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Paises
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Id")]
        public int Id { set; get; }

        [Required(ErrorMessage ="Ingrese el nombre del País")]
        [Display(Name = "País")]
        public string Pais { set; get; }
    }
}