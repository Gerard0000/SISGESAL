using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data;

namespace SISGESAL.web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IEnumerable<SelectListItem> GetComboCourts()
        {
            var list = _dataContext.Courts.Select(type => new SelectListItem
            {
                Text = type.Name,
                Value = $"{type.Id}"
            })
                .OrderBy(type => type.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboDepartments()
        {
            var list = _dataContext.Departments.Select(type => new SelectListItem
            {
                Text = type.Name,
                Value = $"{type.Id}"
            })
                .OrderBy(type => type.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboMunicipalities()
        {
            var list = _dataContext.Municipalities.Select(type => new SelectListItem
            {
                Text = type.Name,
                Value = $"{type.Id}"
            })
                .OrderBy(type => type.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }
    }
}
