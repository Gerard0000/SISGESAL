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
    public class CourtsController : Controller
    {
        private readonly DataContext _dataContext;

        public CourtsController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Courts
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Courts
                .ToListAsync());
        }

        // GET: Courts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _dataContext.Courts
                .Include(d => d.Municipality!)
                .ThenInclude(m => m.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (court == null)
            {
                return NotFound();
            }

            return View(court);
        }

        //PARA COMBOBOX EN DROPDOWNLIST
        [HttpGet("combo/{municipalityId:int}")]
        public async Task<ActionResult> GetCombo(int municipalityId)
        {
            return Ok(await _dataContext.Courts
                .Where(x => x.Municipality!.Id == municipalityId)
                .ToListAsync());
        }

        // GET: Courts/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var municipality = await _dataContext.Municipalities.FindAsync(id.Value);
            if (municipality == null)
            {
                return NotFound();
            }
            var model = new CourtViewModel
            {
                MunicipalityId = municipality.Id
            };
            return View(model);
        }

        // POST: Courts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourtViewModel model)
        {
            if (_dataContext.Courts.Any(x => x.Name == model.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{model.Name}' ya se esta usando";
            }
            else
if (ModelState.IsValid)
            {
                var court = await ToCourtAsync(model, true);

                try
                {
                    _dataContext.Add(court);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Juzgado o Tribunal Agregado Exitosamente";
                    return RedirectToAction("Details", "Municipalities", new { @id = model.MunicipalityId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.ToString());
                }
            }
            return View(model);
        }

        // GET: Courts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _dataContext.Courts
                .Include(x => x.Municipality!)
                .ThenInclude(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (court == null)
            {
                return NotFound();
            }
            return View(ToCourtViewModel(court));
        }

        // POST: Courts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourtViewModel model)
        {
            if (ModelState.IsValid)
            {
                var court = await ToCourt2Async(model, false);
                try
                {
                    _dataContext.Update(court);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Juzgado o Tribunal Editado Exitosamente";
                    return RedirectToAction("Details", "Municipalities", new { @id = model.MunicipalityId });
                }
                catch (Exception)
                {
                    ViewBag.error = "Un error ha ocurrido ó el Código de Municipio esta duplicado con el de otro";
                }
            }
            return View(model);
        }

        // GET: Courts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var court = await _dataContext.Courts
                .Include(d => d.Municipality)
                .ThenInclude(m => m!.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (court == null)
            {
                return NotFound();
            }

            return View(ToCourtViewModel(court));
        }

        // POST: Courts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var court = new CourtViewModel { Id = id };
            if (court != null)
            {
                _dataContext.Courts.Remove(court);
            }

            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageDelete"] = "Juzgado o Tribunal Eliminado Exitosamente";
                return RedirectToAction("Details", "Municipalities", new { @id = court!.MunicipalityId });
            }
            catch (Exception)
            {
                ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con algún usuario de la base de datos.";
            }
            return View("Index , Departments");
        }

        private bool CourtExists(int id)
        {
            return _dataContext.Courts.Any(e => e.Id == id);
        }

        //OTROS METODOS PRIVADOS

        //SE RELACIONA PARA CREAR NUEVA NOTIFICACIÓN
        private async Task<Court> ToCourtAsync(CourtViewModel model, bool isNew)
        {
            var court = new Court

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                Observation = model.Observation?.Trim().ToUpper(),

                Municipality = await _dataContext.Municipalities.FindAsync(model.MunicipalityId),

                CreationDate = DateTime.Now,
                Creator = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return court;
        }

        //SE RELACIONA PARA EDITAR NUEVA NOTIFICACIÓN
        private async Task<Court> ToCourt2Async(CourtViewModel model, bool isNew)
        {
            var court = new Court

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                Observation = model.Observation?.Trim().ToUpper(),

                Municipality = await _dataContext.Municipalities.FindAsync(model.MunicipalityId),

                CreationDate = model.CreationDate,
                Creator = model.Creator,
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return court;
        }

        //SE RELACIONA private PARA EDITAR NOTIFICACIÓN
        private CourtViewModel ToCourtViewModel(Court court)
        {
            return new CourtViewModel
            {
                Name = court.Name?.Trim().ToUpper(),
                Observation = court.Observation?.Trim().ToUpper(),

                Municipality = court.Municipality,

                CreationDate = court.CreationDate,
                Creator = court.Creator,
                ModificationDate = court.ModificationDate,
                Modifier = court.Modifier,

                Id = court.Id,

                MunicipalityId = court.Municipality!.Id,

                //Municipalities = _combosHelper.GetComboMunicipalities(),
            };
        }
    }
}