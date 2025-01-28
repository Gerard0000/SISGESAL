using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SISGESAL.web.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SISGESAL.web.Models
{
    public class EditUserViewModel : User
    {
        public new int Id { get; set; }

        [Display(Name = "Nombre de Usuario")]
        [MaxLength(30, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public new string? UserName { get; set; }

        [Display(Name = "Nombre Completo")]
        [MaxLength(97, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public new string? FullName { get; set; }

        [Display(Name = "DNI")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [MaxLength(13)]
        [MinLength(13, ErrorMessage = "El {0} no puede tener menos de {1} caracteres.")]
        public new string? DNI { get; set; }

        [MaxLength(97)]
        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public new string? Occupation { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(255, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public new string? Email { get; set; }

        [Display(Name = "Número de Teléfono")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(8, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [MinLength(8, ErrorMessage = "El {0} no puede tener menos de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public new string? PhoneNumber { get; set; }

        [MaxLength(255)]
        [Display(Name = "Observación")]
        public new string? Observation { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Departamento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Municipio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int MunicipalityId { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Juzgado o Tribunal")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int CourtId { get; set; }

        [Display(Name = "Almacén")]
        [Range(1, int.MaxValue)]
        public int? DepotId { get; set; }

        public IEnumerable<SelectListItem>? Depots { get; set; }
        public IEnumerable<SelectListItem>? Departments { get; set; }
        public IEnumerable<SelectListItem>? Municipalities { get; set; }
        public IEnumerable<SelectListItem>? Courts { get; set; }
    }
}