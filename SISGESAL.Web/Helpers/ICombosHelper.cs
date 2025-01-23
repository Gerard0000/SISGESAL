using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Helpers
{
    public interface ICombosHelper
    {
        //COMBOBOX DE DEPARTAMENTOS, MUNICIPIOS Y CORTES NORMALES

        //IEnumerable<SelectListItem> GetComboDepartments();

        //IEnumerable<SelectListItem> GetComboMunicipalities();

        //IEnumerable<SelectListItem> GetComboCourts();

        IEnumerable<SelectListItem> GetComboKindofArticles();

        IEnumerable<SelectListItem> GetComboTradeMarks();

        IEnumerable<SelectListItem> GetComboSuppliers();

        //IEnumerable<SelectListItem> GetComboUserWithNoDepot();

        //IEnumerable<SelectListItem> GetComboDepotWithNoUser();


        //probar dropdownlist

        IEnumerable<SelectListItem> GetComboDepartments();

        IEnumerable<SelectListItem> GetComboMunicipalities(int departmentId);

        IEnumerable<SelectListItem> GetComboCourts(int municipalityId);




        Task<Department> GetDepartmentAsync(Municipality municipality);

        Task<Municipality> GetMunicipalityAsync(Court court);

        Task<Municipality> GetCourtAsync(Court court);
    }
}