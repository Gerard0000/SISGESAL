using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //DROPDOWNLIST O COMBOBOX DE DEPARTAMENTO
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

        //DROPDOWNLIST O COMBOBOX DE MUNICIPIO
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

        //DROPDOWNLIST O COMBOBOX DE JUZGADOS O TRIBUNALES
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

        public IEnumerable<SelectListItem> GetComboKindofArticles()
        {
            var list = _dataContext.KindofArticles.Select(type => new SelectListItem
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

        public IEnumerable<SelectListItem> GetComboTradeMarks()
        {
            var list = _dataContext.TradeMarks.Select(type => new SelectListItem
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

        public IEnumerable<SelectListItem> GetComboSuppliers()
        {
            var list = _dataContext.Suppliers.Select(type => new SelectListItem
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

        //public IEnumerable<SelectListItem> GetComboUserWithNoDepot()
        //{
        //    //TODO: REVISAR
        //    var list = _dataContext.Depots.Where(x => x.User != ).Select(type => new SelectListItem
        //    {
        //        Text = type.Name,
        //        Value = $"{type.Id}"
        //    })
        //        .OrderBy(type => type.Text)
        //        .ToList();

        //    list.Insert(0, new SelectListItem
        //    {
        //        Text = "--Seleccione una Opción",
        //        Value = "0"
        //    });

        //    return list;
        //}
    }
}