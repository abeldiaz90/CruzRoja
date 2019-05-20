using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Cobros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Key]
        [Required(ErrorMessage ="El Folio es Requerido")]
        [Display(Name ="Número de Orden:")]
        public int IdFolio { set; get; }

        [Display(Name = "SubTotal:")]
        [DataType(DataType.Currency)]
        public decimal subtotal { set; get; }

        [Display(Name = "IVA:")]
        [DataType(DataType.Currency)]
        public decimal IVA { set; get; }

        [Display(Name = "Total:")]
        [DataType(DataType.Currency)]
        public decimal Total { set; get; }

        [Display(Name = "Denominacion:")]
        [DataType(DataType.Currency)]
        public decimal Denominacion { set; get; }
        [Display(Name = "Requiere Factura:")]
        public Boolean Factura { set; get; }
     
        [Display(Name = "Tipo de Pago:")]
        public Boolean TipoPago { set; get; }

        [Display(Name = "Requiere Descuento:")]
        public Boolean Descuento { set; get; }

        [Display(Name = "Importe de Descuento:")]
        public decimal ImporteDescuento { set; get; }

        [Display(Name = "Justificación:")]
        public String Justificacion { set; get; }

    }
}