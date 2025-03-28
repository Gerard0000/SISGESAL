﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SISGESAL.web.Data.Entities
{
    public class Depot
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Almacén")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        public string? Name { get; set; }

        [Display(Name = "Estado")]
        public bool Status { get; set; }

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

        [Display(Name = "Dirección")]
        public Court? Court { get; set; } = null;

        //[Display(Name = "Usuario")]
        //public ICollection<User>? Users { get; set; }

        [Display(Name = "Usuario")]
        public User? User { get; set; } = null;

        //INTENTAR
        //public int? UserId { get; set; }
    }
}