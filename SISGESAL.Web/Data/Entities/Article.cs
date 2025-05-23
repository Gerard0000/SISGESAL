﻿using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Data.Entities
{
    public class Article
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Display(Name = "Artículo")]
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

        //SOLO ME TRAE LOS SIGUIENTES DATOS CON COMBOBOX

        [Display(Name = "Tipo de Artículo")]
        public KindofArticle? KindofArticle { get; set; }

        [Display(Name = "Marca de Artículos")]
        public TradeMark? TradeMark { get; set; }

        [Display(Name = "Proveedor")]
        public Supplier? Supplier { get; set; }
    }
}
