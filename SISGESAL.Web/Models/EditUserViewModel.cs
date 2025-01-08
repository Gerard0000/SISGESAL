using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Models
{
    public class EditUserViewModel : User
    {
        public int Id { get; set; }

        [Display(Name = "Nombre Completo")]
        [MaxLength(97, ErrorMessage = "El {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string? FullName { get; set; }

        [Display(Name = "DNI")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [MaxLength(13)]
        [MinLength(13, ErrorMessage = "El {0} no puede tener menos de {1} caracteres.")]
        public string? DNI { get; set; }

        [MaxLength(97)]
        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string? Occupation { get; set; }

        [MaxLength(255)]
        [Display(Name = "Observación")]
        public string? Observation { get; set; }

        [NotMapped]
        [Display(Name = "Departamento")]
        public int DepartmentId { get; set; }

        [NotMapped]
        [Display(Name = "Municipios")]
        public int MunicipalityId { get; set; }

        [Required(ErrorMessage = "El {0} es requerido")]
        [Display(Name = "Juzgado o Tribunal")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes Seleccionar un {0}")]
        public int CourtId { get; set; }

        public IEnumerable<SelectListItem>? Departments { get; set; }
        public IEnumerable<SelectListItem>? Municipalities { get; set; }
        public IEnumerable<SelectListItem>? Courts { get; set; }
    }
}
