using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class ServiciosAmbulancias
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { set; get; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha:")]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { set; get; }
        [Required]
        [Display(Name = "Reportante:")]
        public String Reportante { set; get; }
        [Required]
        [Display(Name = "Calle:")]
        public string Calle { set; get; }
        [Required]
        [Display(Name = "Numero Exterior:")]
        public Int32 NumeroExt { set; get; }
        [Required]
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
        public String Telefono { set; get; }
        [Required]
        [Display(Name = "Clave de Emergencia:")]
        public Int32 ClaveEmergencia { set; get; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}",  ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de recepción:")]
        public DateTime HoraRecepcion { set; get; }
        [Required]
        [Display(Name = "Unidad Asignada:")]
        public Int32 UnidadAsignada { set; get; }
        [Required]
        [Display(Name = "Kilometraje:")]
        public Int32 KilometrajeSalida { set; get; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hora de Salida:")]
        public DateTime HoraSalida { set; get; }
    }
}