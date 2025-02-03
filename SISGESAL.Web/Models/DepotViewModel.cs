using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SISGESAL.web.Models
{
    public class DepotViewModel : Depot
    {
        [Display(Name = "Departamento")]
        public int? DepartmentId { get; set; }

        public IEnumerable<SelectListItem>? Departments { get; set; } = null;

        [Display(Name = "Municipio")]
        public int? MunicipalityId { get; set; }

        public IEnumerable<SelectListItem>? Municipalities { get; set; } = null;

        [Display(Name = "Juzgado o Tribunal")]
        public int? CourtId { get; set; }

        public IEnumerable<SelectListItem>? Courts { get; set; } = null;
        //public int UserId { get; set; }
    }
}