﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class OrdenesTemporal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [Required(ErrorMessage = "Folio")]
        public int IdFolio { set; get; }
        [Required(ErrorMessage = "Servicio")]
        [Display(Name = "Servicio:")]
        public int IdServicio { set; get; }

        [Required(ErrorMessage = "Precio")]
        [Display(Name = "Precio:")]
        public int IdPrecio { set; get; }

        [Required(ErrorMessage = "Indique la cantidad")]
        [Display(Name = "Cantidad:")]
        public int cantidad { set; get; }
    }
}