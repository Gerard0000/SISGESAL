using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDepartments();

        IEnumerable<SelectListItem> GetComboMunicipalities();

        IEnumerable<SelectListItem> GetComboCourts();

        //probar dropdownlist
        //IEnumerable<SelectListItem> GetComboMunicipalities(int departmentId);

        //IEnumerable<SelectListItem> GetComboCourts(int municipalityId);

        //Task<Department> GetDepartmentAsync(Municipality municipality);

        //Task<Municipality> GetMunicipalityAsync(Court court);
    }
}