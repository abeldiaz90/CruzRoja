using Microsoft.Reporting.WinForms.Internal.Soap.ReportingServices2005.Execution;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Claves
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { set; get; }

        [Required]
        [Display(Name ="Clave de Emergencia:")]
        public String Clave { set; get; }

        [Required]
        [Display(Name = "Descripción:")]
        public String Descripcion { set; get; }
    }
            
}