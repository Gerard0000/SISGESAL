using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Data.Entities
{
    public class Court
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Juzgado o Tribunal")]
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

        [Display(Name = "Municipio")]
        public Municipality? Municipality { get; set; }

        public ICollection<User>? Users { get; set; }

        //CONTARME JUZGADOS
        //[Display(Name = "Número de Usuarios")]
        //public int UsersNumber => Users == null ? 0 : Users.Count;
    }
}
