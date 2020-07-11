using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace mvc.Models
{
    public class ServiciosAmbulancias
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Display(Name ="Folio:")]
        public Int32 Id { set; get; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha:")]
        [DataType(DataType.Date)]
        public DateTime Fecha { set; get; }
        [Required(ErrorMessage ="Ingrese el Nombre del reportante")]
        [Display(Name = "Reportante:")]
        public String Reportante { set; get; }
        [Required]
        [Display(Name = "Calle:")]
        public string Calle { set; get; }
        [Required(ErrorMessage ="Indique Número Exterior")]
        [Display(Name = "Numero Exterior:")]
        public Int32 NumeroExt { set; get; }

        [ForeignKey("IdColonia")]
        public IEnumerable<Colonias> colonias { set; get; }
        [Required(ErrorMessage ="Indique la Colonia")]
        [Display(Name = "Colonia:")]
        public Int32 IdColonia { set; get; }

        [Required]
        [Display(Name = "Cruzamientos:")]
        public String Cruzamientos { set; get; }
        [Required]
        [Display(Name = "Referencia:")]
        public String Referencia { set; get; }
        [Required]
        [Display(Name = "Telefono:")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Telefono Invalido")]
        public String Telefono { set; get; }

        [ForeignKey("IdClave")]
        public IEnumerable<Claves> claves { set; get; }
        [Required]
        [Display(Name = "Clave de Emergencia:")]
        public Int32 IdClave { set; get; }


        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Recepción:")]
        public DateTime HoraRecepcion { set; get; }

        [ForeignKey("IdAmbulancia")]
        public IEnumerable<Ambulancias> ambulancias { set; get; }

        [Required]
        [Display(Name = "Unidad Asignada:")]
        public Int32 IdAmbulancia { set; get; }
        [Required]
        [Display(Name = "Kilometraje de Salida de Base:")]
        public Int32 KilometrajeSalida { set; get; }       
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Salida de Base:")]
        public DateTime HoraSalida { set; get; }
        [Required]
        [Display(Name = "Hora de Llegada a Sitio:")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime HoraLlegadaSitio { set; get; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Traslado:")]
        public DateTime HoraTraslado { set; get; }

        [ForeignKey("IdTraslado")]
        public IEnumerable<Traslados> traslados { set; get; }
        [Required]
        [Display(Name = "Lugar de Traslado:")]
        public Int32 IdTraslado { set; get; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Fin de Traslado:")]
        public DateTime HoraSalidaTraslado { set; get; }

        [Required]
        [Display(Name = "Kilometraje de Llegada a Base:")]
        public Int32 KilometrajeLlegada { set; get; }
        [Required]
        [Display(Name = "Kilometraje Recorrido:")]
        public Int32 KilometrajeRecorrido { set; get; }

        [Display(Name = "Status:")]
        public String Status { set; get; }
    }
}
