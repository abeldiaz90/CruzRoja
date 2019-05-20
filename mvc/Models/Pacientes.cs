using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class Pacientes
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { set; get; }
        [Required(ErrorMessage = "Escriba Nombre del Paciente")]
        [Display(Name = "Primer Nombre:")]
        public String Nombre { set; get; }
        [Display(Name = "Segundo Nombre:")]
        public String SegundoNombre { set; get; }

        [Display(Name = "Apellido Paterno:")]
        [Required(ErrorMessage = "Escriba Apellido Paterno del Paciente")]
        public String ApellidoPaterno { set; get; }

        [Display(Name = "Apellido Materno:")]
        public String ApellidoMaterno { set; get; }
        [Display(Name = "Fecha de Nacimiento:")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { set; get; }
        [Display(Name = "RFC:")]
        [StringLength(13, ErrorMessage = "Máximo {1} caracteres")]
        [StringValidator(MinLength = 13, MaxLength = 13)]
        public String RFC { set; get; }

        [Display(Name = "CURP:")]
        [StringLength(18, ErrorMessage = "Curp Invalida")]
        public String CURP { set; get; }


        [Display(Name = "Pacientes:")]
        public IEnumerable<Pacientes> pacientes { set; get; }

        [Display(Name = "Sexo:")]
        [Required(ErrorMessage = "Indique sexo del paciente:")]
        public int IdSexo { set; get; }

        [Display(Name = "Seleccione el sexo")]
        public SexoLista Sexo { set; get; }

    
        public enum SexoLista
        {
            [Display(Name = "Masculino")]
            Masculino,
            [Display(Name = "Femenino")]
            Femenino
        };

    

    }
}