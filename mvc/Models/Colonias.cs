using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Colonias
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int32 IdColonia { set; get; }
        [Required]
        [Display(Name = "Colonia:")]
        public String Colonia { set; get; }
    }
}