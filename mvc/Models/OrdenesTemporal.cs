using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Display(Name = "SubTotal:")]
        public decimal subtotal { set; get; }

        [Display(Name = "IVA:")]
        public decimal IVA { set; get; }

        [Display(Name = "Total:")]
        public decimal Total { set; get; }

        [Display(Name = "Paga con:")]
        public decimal PagaCon { set; get; }

        [Display(Name = "Cambio:")]
        public decimal cambio { set; get; }

        [Display(Name = "Elemento ya fue agregado")]
        public Boolean Agregado { set; get; }
    }
}