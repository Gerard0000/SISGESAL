using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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



        //DROPDOWNLIST O COMBOBOX DE MUNICIPIO
        //public IEnumerable<SelectListItem> GetComboMunicipalities()
        //{
        //    var list = _dataContext.Municipalities.Select(type => new SelectListItem
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

        //DROPDOWNLIST O COMBOBOX DE JUZGADOS O TRIBUNALES
        //public IEnumerable<SelectListItem> GetComboCourts()
        //{
        //    var list = _dataContext.Courts.Select(type => new SelectListItem
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

        //PROBAR COMBOBOX DE DEPARTMENT, MUNICIPALITIES AND COURTS

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

        public IEnumerable<SelectListItem> GetComboMunicipalities(int departmentId)
        {
            var department = _dataContext.Departments.Find(departmentId);
            var list = new List<SelectListItem>();
            if (department != null)
            {
                list = department.Municipalities.Select(type => new SelectListItem
                {
                    Text = type.Name,
                    Value = $"{type.Id}"
                })
                .OrderBy(type => type.Text)
                .ToList();
            }
            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboCourts(int municipalityId)
        {
            var municipality = _dataContext.Municipalities.Find(municipalityId);
            var list = new List<SelectListItem>();
            if (municipality != null)
            {
                list = municipality.Courts.Select(type => new SelectListItem
                {
                    Text = type.Name,
                    Value = $"{type.Id}"
                })
                .OrderBy(type => type.Text)
                .ToList();
            }
            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        public async Task<Department> GetDepartmentAsync(Municipality municipality)
        {
            return await _dataContext.Departments
                .Where(d => d.Municipalities.Any(m => m.Id == m.Id))
                .FirstOrDefaultAsync();
        }

        public async Task<Municipality> GetMunicipalityAsync(Court court)
        {
            return await _dataContext.Municipalities
                .Where(d => d.Courts.Any(m => m.Id == m.Id))
                .FirstOrDefaultAsync();
        }

        public async Task<Court> GetCourtAsync(int id)
        {
            return await _dataContext.Courts.FindAsync(id);
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