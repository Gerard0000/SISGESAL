using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using System.Diagnostics.Metrics;

namespace SISGESAL.web.Helpers
{
    public class CombosHelper(DataContext dataContext) : ICombosHelper
    {
        private readonly DataContext _dataContext = dataContext;

        //DROPDOWNLIST O COMBOBOX DE TIPO DE ARTÍCULO
        public IEnumerable<SelectListItem> GetComboKindofArticles()
        {
            var list = _dataContext.KindofArticles.Where(type => type.Status == true).Select(type => new SelectListItem
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

        //DROPDOWNLIST O COMBOBOX DE MARCA DE ARTÍCULO
        public IEnumerable<SelectListItem> GetComboTradeMarks()
        {
            var list = _dataContext.TradeMarks.Where(type => type.Status == true).Select(type => new SelectListItem
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

        //DROPDOWNLIST O COMBOBOX DE PROVEEDOR DE ARTÍCULO
        public IEnumerable<SelectListItem> GetComboSuppliers()
        {
            var list = _dataContext.Suppliers.Where(type => type.Status == true).Select(type => new SelectListItem
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
        public IEnumerable<SelectListItem> GetComboMunicipalities(int departmentId)
        {
            var department = _dataContext.Departments.Find(departmentId);
            var list = new List<SelectListItem>();
            if (department != null)
            {
                list = [.. department.Municipalities!.Select(type => new SelectListItem
                {
                    Text = type.Name,
                    Value = $"{type.Id}"
                })
                //SE SIMPLIFICÓ Y NO LLEVA TOLIST PORQUE PIDE UN PARÁMETRO
                .OrderBy(type => type.Text)];
            }
            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        //DROPDOWNLIST O COMBOBOX DE JUZGADO O TRIBUNAL
        public IEnumerable<SelectListItem> GetComboCourts(int municipalityId)
        {
            var municipality = _dataContext.Municipalities.Find(municipalityId);
            var list = new List<SelectListItem>();
            if (municipality != null)
            {
                list = [.. municipality.Courts!.Select(type => new SelectListItem
                {
                    Text = type.Name,
                    Value = $"{type.Id}"
                })
                //SE SIMPLIFICÓ Y NO LLEVA TOLIST PORQUE PIDE UN PARÁMETRO
                .OrderBy(type => type.Text)];
            }
            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        //DROPDOWNLIST O COMBOBOX DE ALMACÉN
        public IEnumerable<SelectListItem> GetComboDepots(int courtId)
        {
            var court = _dataContext.Courts.Find(courtId);
            var list = new List<SelectListItem>();
            if (court != null)
            {
                list = [.. court.Depots!.Select(type => new SelectListItem
                {
                    Text = type.Name,
                    Value = $"{type.Id}"
                })
                //SE SIMPLIFICÓ Y NO LLEVA TOLIST PORQUE PIDE UN PARÁMETRO
                .OrderBy(type => type.Text)];
            }
            list.Insert(0, new SelectListItem
            {
                Text = "--Seleccione una Opción",
                Value = "0"
            });

            return list;
        }

        //TODO:*********************ELIMINAR CUANDO SE ARREGLE EL AJAX Y ENCUENTRE SOLUCIÓN**********************

        //DROPDOWNLIST O COMBOBOX **PROVISIONAL** DE MUNICIPIOS
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

        //DROPDOWNLIST O COMBOBOX **PROVISIONAL** DE JUZGADOS O TRIBUNALES
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

        //DROPDOWNLIST O COMBOBOX **PROVISIONAL** DE ALMACENES
        public IEnumerable<SelectListItem> GetComboDepots()
        {
            var list = _dataContext.Depots.Where(type => type.Status == true).Select(type => new SelectListItem
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

        //**************************POR LOS MOMENTOS AQUI TERMINA**********************







        //public async Task<Department> GetDepartmentAsync(Municipality municipality)
        //{
        //    return await _dataContext.Departments
        //        .Where(d => d.Municipalities.Any(m => m.Id == municipality.Id))
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<Municipality> GetMunicipalityAsync(Court court)
        //{
        //    return await _dataContext.Municipalities
        //        .Where(d => d.Courts.Any(m => m.Id == court.Id))
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<Municipality> GetMunicipalityAsync(int id)
        //{
        //    return await _dataContext.Municipalities.FindAsync(id);
        //}

        //public async Task<Court> GetCourtAsync(int id)
        //{
        //    return await _dataContext.Courts.FindAsync(id);
        //}

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





        //PROBAR COMBOBOX DE DEPARTMENT, MUNICIPALITIES AND COURTS

        //****************************************************************

        //****************************************************************

        //****************************************************************para el json
        //public async Task<Department> GetDepartmentWithMunicipalityAsync(int id)
        //{
        //    return await _dataContext.Departments
        //        .Include(c => c.Municipalities)
        //        .Where(c => c.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<Municipality> GetMunicipalityWithCourtsAsync(int id)
        //{
        //    return await _dataContext.Municipalities
        //        .Include(c => c.Courts)
        //        .Where(c => c.Id == id)
        //        .FirstOrDefaultAsync();
        //}

        //Task<Court> ICombosHelper.GetMunicipalityAsync(int municipalityId)
        //{
        //    throw new NotImplementedException();
        //}

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