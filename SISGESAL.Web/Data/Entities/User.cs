using Microsoft.AspNetCore.Identity;
using SISGESAL.web.Enums;
using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Data.Entities
{
    public class User : IdentityUser
    {
        //CREAMOS LAS PROPIEDADES A EXPORTAR A LA BASE DE DATOS
        [Required]
        [MaxLength(97)]
        [Display(Name = "Nombre Completo")]
        public string? FullName { get; set; } = null;

        [Display(Name = "DNI")]
        [MaxLength(13)]
        [MinLength(13, ErrorMessage = "El {0} no puede tener menos de {1} caracteres.")]
        public string? DNI { get; set; } = null;

        [MaxLength(97)]
        [Display(Name = "Cargo")]
        public string? Occupation { get; set; } = null;

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        [MaxLength(255)]
        [Display(Name = "Observación")]
        public string? Observation { get; set; } = null;

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Creador")]
        public string? Creator { get; set; }

        [Display(Name = "Fecha de Modificación")]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime ModificationDate { get; set; }

        [Display(Name = "Modificador")]
        public string? Modifier { get; set; }

        //PROPIEDAD DE LECTURA DE FECHA, INTENTO DE HACERLA INTERNACIONAL

        [Display(Name = "Fecha de Creación")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime DateLocalCreation => CreationDate.ToLocalTime();

        [Display(Name = "Fecha de Modificación")]
        //[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd", ApplyFormatInEditMode = true)]
        public DateTime DateLocalModification => ModificationDate.ToLocalTime();

        //SOLO ME TRAE LOS SIGUIENTES DATOS CON COMBOBOX

        [Display(Name = "Juzgado o Tribunal")]
        public Court? Court { get; set; }
    }
}
