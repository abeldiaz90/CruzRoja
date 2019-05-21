using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Estados
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }        
        public IEnumerable<Paises> pais { set; get; }

        [ForeignKey("IdPais")]
        public Paises paises { set; get; }

        [Required(ErrorMessage = "Seleccione el Pais")]
        [Display(Name = "Pais")]
        public int IdPais { set; get; }

        [Required(ErrorMessage = "Ingrese el nombre del Estado")]
        [Display(Name = "Estado")]
        public string Estado { set; get; }
    }
}