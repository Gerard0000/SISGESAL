using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Controllers
{
    public class AlmacensController : Controller
    {
        private readonly DataContext _dataContext;

        public AlmacensController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Almacens
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Almacens.Count();

            var almacenes = await _dataContext.Almacens
                .Select(a => new Almacen
                {
                    Name = a.Name,
                }).ToListAsync();

            return View(almacenes);
        }

        // GET: Almacens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _dataContext.Almacens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (almacen == null)
            {
                return NotFound();
            }

            return View(almacen);
        }

        // GET: Almacens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Almacens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Estado,Observation,CreationDate,Creator,ModificationDate,Modifier")] Almacen almacen)
        {
            //VALOR DUPLICADO
            if (_dataContext.Almacens.Any(x => x.Name == almacen.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{almacen.Name}' ya se esta usando";
            }
            if (ModelState.IsValid)
            {
                almacen.Name = almacen.Name?.ToUpper().Trim();
                almacen.Observation = almacen.Observation?.ToUpper().Trim();
                almacen.Estado = true;
                almacen.CreationDate = DateTime.Now;
                almacen.ModificationDate = DateTime.Now;
                almacen.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                almacen.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    _dataContext.Add(almacen);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Almacén Agregado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(almacen);
        }

        // GET: Almacens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _dataContext.Almacens.FindAsync(id);
            if (almacen == null)
            {
                return NotFound();
            }
            return View(almacen);
        }

        // POST: Almacens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Estado,Observation,CreationDate,Creator,ModificationDate,Modifier")] Almacen almacen)
        {
            if (id != almacen.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    almacen.Name = almacen.Name?.ToUpper().Trim();
                    almacen.Observation = almacen.Observation?.ToUpper().Trim();
                    almacen.ModificationDate = DateTime.Now;
                    almacen.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dataContext.Update(almacen);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Almacén Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlmacenExists(almacen.Id))
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
            return View(almacen);
        }

        // GET: Almacens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var almacen = await _dataContext.Almacens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (almacen == null)
            {
                return NotFound();
            }

            return View(almacen);
        }

        // POST: Almacens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var almacen = await _dataContext.Almacens.FindAsync(id);
            if (almacen != null)
            {
                _dataContext.Almacens.Remove(almacen);
            }

            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageDelete"] = "Almacén Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con alguna notificación o usuario de la base de datos.";
            }
            return View(almacen);
        }

        private bool AlmacenExists(int id)
        {
            return _dataContext.Almacens.Any(e => e.Id == id);
        }
    }
}
