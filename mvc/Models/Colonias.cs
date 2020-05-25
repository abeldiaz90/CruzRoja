using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Colonias
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { set; get; }
        [Required]
        [Display(Name ="Colonia:")]
        public String Colonia { set; get; }
    }
}