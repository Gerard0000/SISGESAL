using Microsoft.AspNetCore.Mvc.Rendering;

namespace SISGESAL.web.Helpers
{
    public interface ICombosHelper
    {
        //SE RELACIONAN CON EL ARTÍCULO

        IEnumerable<SelectListItem> GetComboKindofArticles();

        IEnumerable<SelectListItem> GetComboTradeMarks();

        IEnumerable<SelectListItem> GetComboSuppliers();

        //PARA HACER DROPDOWNLIST EN CASCADA
        IEnumerable<SelectListItem> GetComboOccupations();

        IEnumerable<SelectListItem> GetComboDepartments();

        IEnumerable<SelectListItem> GetComboMunicipalities(int departmentId);

        IEnumerable<SelectListItem> GetComboCourts(int municipalityId);

        IEnumerable<SelectListItem> GetComboDepots(int courtId);
    }
}