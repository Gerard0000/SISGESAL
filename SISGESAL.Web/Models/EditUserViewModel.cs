﻿using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Display(Name = "Departamento")]
        public int? DepartmentId { get; set; }

        [Display(Name = "Municipio")]
        public int? MunicipalityId { get; set; }

        [Display(Name = "Juzgado o Tribunal")]
        public int? CourtId { get; set; }

        [Display(Name = "Almacén")]
        public IEnumerable<SelectListItem>? Depots { get; set; } = null;

        public IEnumerable<SelectListItem>? Departments { get; set; } = null;
        public IEnumerable<SelectListItem>? Municipalities { get; set; } = null;
        public IEnumerable<SelectListItem>? Courts { get; set; } = null;
    }
}