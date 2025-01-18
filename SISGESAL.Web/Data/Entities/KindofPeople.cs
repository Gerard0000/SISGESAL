using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Data.Entities
{
    public class KindofPeople
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Género")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string? Name { get; set; }

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
        public ICollection<Gender>? Genders { get; set; }

        //CONTARME GENDER
        [Display(Name = "Número de Objetos")]
        public int GendersNumber => Genders == null ? 0 : Genders.Count;
    }
}
