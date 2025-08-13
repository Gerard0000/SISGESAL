using Microsoft.AspNetCore.Mvc.Rendering;
using SISGESAL.web.Data;

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

        //DROPDOWNLIST O COMBOBOX DE CARGO O PUESTO DE TRABAJO
        public IEnumerable<SelectListItem> GetComboOccupations()
        {
            var list = _dataContext.Occupations.Select(type => new SelectListItem
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
    }
}