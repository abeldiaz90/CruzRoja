using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Roles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { set; get; }

        [Display(Name ="Rol:")]
        [Required(ErrorMessage ="Escriba el Rol")]
        public String Rol { set; get; }
    }
}