using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DepotsController : Controller
    {
        private readonly DataContext _dataContext;

        public DepotsController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Depots
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Depots.Count();
            ViewBag.Indexcount2 = _dataContext.Depots.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.Depots.Where(m => m.Status == false).Count();
            return View(await _dataContext.Depots.ToListAsync());
        }

        // GET: Depots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depot = await _dataContext.Depots
                .FirstOrDefaultAsync(m => m.Id == id);
            if (depot == null)
            {
                return NotFound();
            }

            return View(depot);
        }

        // GET: Depots/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Depots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] Depot depot)
        {
            //VALOR DUPLICADO
            if (_dataContext.Departments.Any(x => x.Name == depot.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{depot.Name}' ya se esta usando";
            }
            else
            if (ModelState.IsValid)
            {
                depot.Name = depot.Name?.ToUpper().Trim();
                depot.Observation = depot.Observation?.ToUpper().Trim();
                depot.CreationDate = DateTime.Now;
                depot.ModificationDate = DateTime.Now;
                depot.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                depot.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                try
                {
                    _dataContext.Add(depot);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Departamento Agregado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(depot);
        }

        // GET: Depots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depot = await _dataContext.Depots.FindAsync(id);
            if (depot == null)
            {
                return NotFound();
            }
            return View(depot);
        }

        // POST: Depots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] Depot depot)
        {
            if (id != depot.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    depot.Name = depot.Name?.ToUpper().Trim();
                    depot.Observation = depot.Observation?.ToUpper().Trim();
                    depot.CreationDate = DateTime.Now;
                    depot.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    depot.ModificationDate = DateTime.Now;
                    depot.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dataContext.Update(depot);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Departamento Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepotExists(depot.Id))
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
            return View(depot);
        }

        // GET: Managers/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var depot = await _dataContext.Depots
                .FirstOrDefaultAsync(u => u.Id == id);
            if (depot == null)
            {
                return NotFound();
            }
            return View(depot);
        }

        // POST: Managers/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var depot = await _dataContext.Depots
                .FirstOrDefaultAsync(u => u.Id == id);
            if (depot != null)
            {
                depot.Status = true;
                depot.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                depot.ModificationDate = DateTime.Now;
            }

            try
            {
                _dataContext.Update(depot);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageLock"] = "Administrador Bloqueado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Depots/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var depot = await _dataContext.Depots
                .FirstOrDefaultAsync(u => u.Id == id);
            if (depot == null)
            {
                return NotFound();
            }
            return View(depot);
        }

        // POST: Depots/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var depot = await _dataContext.Depots
                .FirstOrDefaultAsync(u => u.Id == id);
            if (depot != null)
            {
                depot.Status = true;
                depot.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                depot.ModificationDate = DateTime.Now;
            }
            try
            {
                _dataContext.Update(depot);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageUnLock"] = "Almacen Activado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool DepotExists(int id)
        {
            return _dataContext.Depots.Any(e => e.Id == id);
        }
    }
}
