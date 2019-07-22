using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class OrdenesDetalles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Display(Name ="Numero de orden")]
        public int IdFolio { set; get; }

        [Display(Name = "Servicio")]
        public int IdServicio { set; get; }

        [Display(Name = "Cantidad:")]
        public int cantidad { set; get; }


        [Display(Name = "SubTotal:")]
        public decimal subtotal { set; get; }

        [Display(Name = "IVA:")]
        public decimal IVA { set; get; }

        [Display(Name = "Total:")]
        public decimal Total { set; get; }

    }
}