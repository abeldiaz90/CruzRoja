using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [Required(ErrorMessage = "El usuario es Obligatorio")]
        [Display(Name = "Usuario:")]
        public string Usuario { set; get; }

        [Required(ErrorMessage = "El password es Obligatorio")]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [Required(ErrorMessage = "El Correo es Obligatorio")]
        [Display(Name = "Correo Electronico:")]
        [DataType(DataType.EmailAddress)]
        public string Correo { set; get; }

        [ForeignKey("IdDelegacion")]
        public Delegaciones delegacion { set; get; }

        [Required(ErrorMessage = "Seleccione su Delegación")]
        [Display(Name = "Delegación:")]
        public int IdDelegacion { set; get; }

        [ForeignKey("IdRol")]
        public Roles roles { set; get; }

        [Required(ErrorMessage = "Seleccione su Delegación")]
        [Display(Name = "Delegación:")]
        public int IdRol { set; get; }

        public IEnumerable<Delegaciones> delegaciones { set; get; }
        public IEnumerable<Users> usuarios { set; get; }
        public IEnumerable<Roles> roleslista { set; get; }
    }
}