using Microsoft.AspNetCore.Mvc.Rendering;

namespace SISGESAL.web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDepartments();

        IEnumerable<SelectListItem> GetComboMunicipalities();

        IEnumerable<SelectListItem> GetComboCourts();
    }
}
