using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data.Entities;
using System.Diagnostics.Metrics;

namespace SISGESAL.web.Helpers
{
    public interface ICombosHelper
    {
        //SE RELACIONAN CON EL ARTÍCULO
        IEnumerable<SelectListItem> GetComboKindofArticles();

        IEnumerable<SelectListItem> GetComboTradeMarks();

        IEnumerable<SelectListItem> GetComboSuppliers();

        //PARA HACER DROPDOWNLIST EN CASCADA

        IEnumerable<SelectListItem> GetComboDepartments();

        IEnumerable<SelectListItem> GetComboMunicipalities(int departmentId);

        IEnumerable<SelectListItem> GetComboCourts(int municipalityId);

        //AUXILIARES DEL DROPDOWNLIST EN CASCADA

        Task<Department> GetDepartmentAsync(Municipality municipality);

        Task<Municipality> GetMunicipalityAsync(Court court);

        Task<Court> GetCourtAsync(int courtId);

        Task<Court> GetMunicipalityAsync(int municipalityId);

        //COMBOBOX DE DEPARTAMENTOS, MUNICIPIOS Y CORTES NORMALES

        //IEnumerable<SelectListItem> GetComboDepartments();

        //IEnumerable<SelectListItem> GetComboMunicipalities();

        IEnumerable<SelectListItem> GetComboCourts();

        //IEnumerable<SelectListItem> GetComboUserWithNoDepot();

        //IEnumerable<SelectListItem> GetComboDepotWithNoUser();

        //****************************************************************

        //probar dropdownlist

        //****************************************************************

        //****************************************************************

        //****************************************************************

        //Task<Municipality> GetCourtAsync(Court court);
        //Task<Court> GetCourtAsync(Court court);

        //**************************************************************para el drodownlist

        Task<Department> GetDepartmentWithMunicipalityAsync(int id);

        Task<Municipality> GetMunicipalityWithCourtsAsync(int id);
    }
}