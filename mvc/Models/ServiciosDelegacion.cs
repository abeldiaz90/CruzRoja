using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class ServiciosDelegacion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Id")]
        public int Id { set; get; }

        [ForeignKey("IdDelegacion")]
        public IEnumerable<Delegaciones> delegaciones { set; get; }
        [ForeignKey("IdServicio")]
        public IEnumerable<Servicios> servicios { set; get; }

        [Required(ErrorMessage = "Seleccione la delegación")]
        [Display(Name = "Delegación:")]
        public int IdDelegacion { set; get; }

        [Required(ErrorMessage = "Seleccione el tipo de servicio")]
        [Display(Name = "Tipo de servicio:")]
        public int IdServicio { set; get; }

        [Required(ErrorMessage = "Seleccione el tipo de servicio")]
        [Display(Name = "Tipo de servicio:")]
        public String NombreServicio { set; get; }


        [Display(Name = "Aplica IVA")]
        public Boolean AplicaIVA { set; get; }

        public IEnumerable<ServiciosDelegacionPrecios> precios { set; get; }
        public IEnumerable<ServiciosVista> serviciosdelegacionvista { set; get; }

        //[Required(ErrorMessage = "Indique el Precio")]
        //[Display(Name = "Precio sin IVA:")]
        //[DataType(DataType.Currency)]
        //public decimal PreciosinIVA { set; get; }

    }
}