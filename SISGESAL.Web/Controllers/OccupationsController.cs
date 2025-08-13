using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SISGESAL.web.Controllers;

[Authorize(Roles = "Manager")]
public class OccupationsController(DataContext context) : Controller
{
    private readonly DataContext _dataContext = context;

    // GET: Occupations
    public async Task<IActionResult> Index()
    {
        ViewBag.Indexcount = _dataContext.Occupations.Count();
        return View(await _dataContext.Occupations
            .ToListAsync());
    }

    // GET: Occupations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var occupation = await _dataContext.Occupations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (occupation == null)
        {
            return NotFound();
        }

        return View(occupation);
    }

    // GET: Occupations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Occupations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Observation,CreationDate,Creator,ModificationDate,Modifier")] Occupation occupation)
    {
        //VALOR DUPLICADO
        if (_dataContext.Occupations.Any(x => x.Name == occupation.Name))
        {
            ViewBag.DuplicateMessage = $"El nombre '{occupation.Name}' ya se esta usando";
        }
        else
        if (ModelState.IsValid)
        {
            occupation.Name = occupation.Name?.ToUpper().Trim();
            occupation.Observation = occupation.Observation?.ToUpper().Trim();
            occupation.CreationDate = DateTime.Now;
            occupation.ModificationDate = DateTime.Now;
            occupation.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
            occupation.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                _dataContext.Add(occupation);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageCreate"] = "Cargo o Puesto de Trabajo Agregado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
            }
        }
        return View(occupation);
    }

    // GET: Occupations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var occupation = await _dataContext.Occupations.FindAsync(id);
        if (occupation == null)
        {
            return NotFound();
        }
        return View(occupation);
    }

    // POST: Occupations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Observation,CreationDate,Creator,ModificationDate,Modifier")] Occupation occupation)
    {
        if (id != occupation.Id)
        {
            return NotFound();
        }

        try
        {
            if (ModelState.IsValid)
            {
                occupation.Name = occupation.Name?.ToUpper().Trim();
                occupation.Observation = occupation.Observation?.ToUpper().Trim();
                occupation.CreationDate = occupation.CreationDate;
                occupation.Creator = occupation.Creator;
                occupation.ModificationDate = DateTime.Now;
                occupation.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _dataContext.Update(occupation);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageEdit"] = "Cargo o Puesto de Trabajo Editado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OccupationExists(occupation.Id))
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
        return View(occupation);
    }

    // GET: Occupations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var occupation = await _dataContext.Occupations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (occupation == null)
        {
            return NotFound();
        }

        return View(occupation);
    }

    // POST: Occupations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var occupation = await _dataContext.Occupations.FindAsync(id);
        if (occupation != null)
        {
            _dataContext.Occupations.Remove(occupation);
        }
        try
        {
            await _dataContext.SaveChangesAsync();
            TempData["AlertMessageDelete"] = "Cargo o Puesto de Trabajo Eliminado Exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con algún otra instancia de la base de datos.";
        }
        return RedirectToAction(nameof(Index));
    }

    private bool OccupationExists(int id)
    {
        return _dataContext.Occupations.Any(e => e.Id == id);
    }
}