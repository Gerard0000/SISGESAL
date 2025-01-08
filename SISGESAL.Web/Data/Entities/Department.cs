using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Data.Entities
{
    public class Department
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string? Name { get; set; }

        [MaxLength(2)]
        [MinLength(2, ErrorMessage = "El {0} no puede tener menos de {1} carácteres")]
        [Display(Name = "Código de Departamento")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string? CodDepHn { get; set; }

        [MaxLength(255)]
        [Display(Name = "Observación")]
        public string? Observation { get; set; }

        [Display(Name = "Fecha de Creación")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [Display(Name = "Creador")]
        public string? Creator { get; set; }

        [Display(Name = "Fecha de Modificación")]
        [DataType(DataType.DateTime)]
        public DateTime ModificationDate { get; set; }

        [Display(Name = "Modificador")]
        public string? Modifier { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime DateLocalCreation => CreationDate.ToLocalTime();

        [Display(Name = "Fecha de Modificación")]
        public DateTime DateLocalModification => ModificationDate.ToLocalTime();

        public ICollection<Municipality>? Municipalities { get; set; }

        //CONTARME MUNICIPIOS
        [Display(Name = "Número de Municipios")]
        public int MunicipalitiesNumber => Municipalities == null ? 0 : Municipalities.Count;
    }
}
