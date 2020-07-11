using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Roles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { set; get; }

        [Display(Name = "Rol:")]
        [Required(ErrorMessage = "Escriba el Rol")]
        public String Rol { set; get; }
    }
}