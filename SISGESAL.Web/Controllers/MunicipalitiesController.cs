using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Models;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class MunicipalitiesController : Controller
    {
        private readonly DataContext _dataContext;

        public MunicipalitiesController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Municipalities
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Courts.Count();
            return View(await _dataContext.Municipalities
                .Include(x => x.Courts)
                .ToListAsync());
        }

        // GET: Municipalities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipality = await _dataContext.Municipalities
                .Include(x => x.Courts!)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipality == null)
            {
                return NotFound();
            }

            return View(municipality);
        }

        // GET: Municipalities/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _dataContext.Departments.FindAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            var model = new MunicipalityViewModel
            {
                DepartmentId = department.Id
            };
            return View(model);
        }

        // POST: Municipalities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MunicipalityViewModel model)
        {
            if (_dataContext.Municipalities.Any(x => x.CodMunHn == model.CodMunHn))
            {
                ViewBag.DuplicateMessage = $"El código '{model.CodMunHn}' ya se esta usando";
            }
            else
            if (ModelState.IsValid)
            {
                var municipality = await ToMunicipalAsync(model, true);

                try
                {
                    _dataContext.Add(municipality);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Municipio Agregado Exitosamente";
                    return RedirectToAction("Details", "Departments", new { @id = model.DepartmentId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.ToString());
                }
            }

            return View(model);
        }

        // GET: Municipalities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipality = await _dataContext.Municipalities
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipality == null)
            {
                return NotFound();
            }
            return View(ToMunicipalityViewModel(municipality));
        }

        // POST: Municipalities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MunicipalityViewModel model)
        {
            if (ModelState.IsValid)
            {
                //UTILIZAMOS METODO PRIVADO QUE ESTA ABAJO PARA LA EDICIÓN DEL MUNICIPIO
                var municipality = await ToMunicipal2Async(model, false);
                try
                {
                    _dataContext.Update(municipality);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Municipio Editado Exitosamente";
                    return RedirectToAction("Details", "Departments", new { @id = model.DepartmentId });
                }
                catch (Exception)
                {
                    ViewBag.error = "Un error ha ocurrido ó el Código de Municipio esta duplicado con el de otro";
                }
            }
            return View(model);
        }

        // GET: Municipalities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipality = await _dataContext.Municipalities
                 .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipality == null)
            {
                return NotFound();
            }

            return View(ToMunicipalityViewModel(municipality));
        }

        // POST: Municipalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var municipality = new MunicipalityViewModel { Id = id };
            //var municipality = await _dataContext.Municipalities.FindAsync(id);
            if (municipality != null)
            {
                _dataContext.Municipalities.Remove(municipality);
            }

            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageDelete"] = "Departamento Eliminado Exitosamente";
                return RedirectToAction("Details", "Departments", new { @id = municipality.DepartmentId });
            }
            catch (Exception)
            {
                ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con algún Juzgado o Tribunal de la base de datos.";
            }
            return View("Index , Departments");
        }

        private bool MunicipalityExists(int id)
        {
            return _dataContext.Municipalities.Any(e => e.Id == id);
        }

        //OTROS METODOS PRIVADOS

        //SE RELACIONA PARA CREAR NUEVO MUNICIPIO
        private async Task<Municipality> ToMunicipalAsync(MunicipalityViewModel model, bool isNew)
        {
            var municipal = new Municipality

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                CodMunHn = model.CodMunHn?.Trim().ToUpper(),
                Observation = model.Observation?.Trim().ToUpper(),

                Department = await _dataContext.Departments.FindAsync(model.DepartmentId),

                CreationDate = DateTime.Now,
                Creator = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return municipal;
        }

        //SE RELACIONA PARA EDITAR NUEVO MUNICIPIO
        private async Task<Municipality> ToMunicipal2Async(MunicipalityViewModel model, bool isNew)
        {
            var municipality = new Municipality

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                CodMunHn = model.CodMunHn?.Trim().ToUpper(),
                Observation = model.Observation?.Trim().ToUpper(),

                Department = await _dataContext.Departments.FindAsync(model.DepartmentId),

                CreationDate = model.CreationDate,
                Creator = model.Creator,
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return municipality;
        }

        //SE RELACIONA private PARA EDITAR MUNICIPIO

        private MunicipalityViewModel ToMunicipalityViewModel(Municipality municipality)
        {
            return new MunicipalityViewModel
            {
                Name = municipality.Name?.Trim().ToUpper(),
                CodMunHn = municipality.CodMunHn?.Trim().ToUpper(),
                Observation = municipality.Observation?.Trim().ToUpper(),

                Department = municipality.Department,

                CreationDate = municipality.CreationDate,
                Creator = municipality.Creator,
                ModificationDate = municipality.ModificationDate,
                Modifier = municipality.Modifier,

                Id = municipality.Id,

                DepartmentId = municipality.Department.Id,

                //Departments = _combosHelper.GetComboDepartments(),
            };
        }
    }
}