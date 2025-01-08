using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly DataContext _dataContext;

        public DepartmentsController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Departments.Count();
            return View(await _dataContext.Departments
                .Include(x => x.Municipalities)
                .ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _dataContext.Departments
                .Include(x => x.Municipalities!)
                .ThenInclude(c => c.Courts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CodDepHn,Observation,CreationDate,Creator,ModificationDate,Modifier")] Department department)
        {
            //VALOR DUPLICADO
            if (_dataContext.Departments.Any(x => x.Name == department.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{department.Name}' ya se esta usando";
            }
            else
            if (_dataContext.Departments.Any(x => x.CodDepHn == department.CodDepHn))
            {
                ViewBag.DuplicateMessage = $"El código '{department.CodDepHn}' ya se esta usando";
            }
            else
            if (ModelState.IsValid)
            {
                department.Name = department.Name?.ToUpper().Trim();
                department.CodDepHn = department.CodDepHn?.ToUpper().Trim();
                department.Observation = department.Observation?.ToUpper().Trim();
                department.CreationDate = DateTime.Now;
                department.ModificationDate = DateTime.Now;
                department.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                department.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                try
                {
                    _dataContext.Add(department);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Departamento Agregado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _dataContext.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CodDepHn,Observation,CreationDate,Creator,ModificationDate,Modifier")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    department.Name = department.Name?.ToUpper().Trim();
                    department.CodDepHn = department.CodDepHn?.ToUpper().Trim();
                    department.Observation = department.Observation?.ToUpper().Trim();
                    department.ModificationDate = DateTime.Now;
                    department.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dataContext.Update(department);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Departamento Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(department.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception)
            {
                ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _dataContext.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _dataContext.Departments.FindAsync(id);
            if (department != null)
            {
                _dataContext.Departments.Remove(department);
            }
            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageDelete"] = "Departamento Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con alguna notificación o usuario de la base de datos.";
            }
            return View(department);
        }

        private bool DepartmentExists(int id)
        {
            return _dataContext.Departments.Any(e => e.Id == id);
        }
    }
}
