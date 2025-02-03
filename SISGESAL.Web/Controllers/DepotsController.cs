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
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DepotsController(DataContext context, ICombosHelper combosHelper) : Controller
    {
        private readonly DataContext _dataContext = context;
        private readonly ICombosHelper _combosHelper = combosHelper;

        //TODO:*********************************************MODIFICAR GRUESO***********************************
        // GET: Depots
        public async Task<IActionResult> Index()
        {
            //ALMACENES ACTIVOS
            ViewBag.Indexcount = _dataContext.Depots.Where(x => x.Status == true).Count();
            //ALMACENES BLOQUEADOS
            ViewBag.Indexcount2 = _dataContext.Depots.Where(m => m.Status == false).Count();
            //TOTAL DE ALMACENES
            ViewBag.Indexcount3 = _dataContext.Depots.Count();
            //USUARIOS ACTIVOS CON ALMACÉN ACTIVO
            ViewBag.Indexcount4 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)).Count();
            //USUARIOS BLOQUEADOS CON ALMACÉN ACTIVO
            ViewBag.Indexcount5 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == true)).Count();
            //TOTAL DE USUARIOS CON ALMACÉN ACTIVO
            ViewBag.Indexcount6 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.Depot!.Status == true)).Count();
            //USUARIOS ACTIVOS SIN ALMACÉN
            ViewBag.Indexcount7 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.User.LockoutEnd == null)).Count();
            //USUARIOS BLOQUEADOS SIN ALMACÉN
            ViewBag.Indexcount8 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.User!.LockoutEnd != null)).Count();
            //TOTAL DE USUARIOS SIN ALMACÉN
            ViewBag.Indexcount9 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.Depot == null).Count();
            //USUARIOS ACTIVOS CON ALMACÉN BLOQUEADO
            ViewBag.Indexcount10 = _dataContext.Customers.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd == null)).Count();
            //USUARIOS BLOQUEADOS CON ALMACÉN BLOQUEADO
            ViewBag.Indexcount11 = _dataContext.Customers.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd > DateTime.Now)).Count();
            //TOTAL DE USUARIOS CON ALMACÉN BLOQUEADO
            ViewBag.Indexcount12 = _dataContext.Customers.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false)).Count();

            return View(await _dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .ToListAsync());
        }

        //*****************************************************************************************************

        // GET: Depots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depot = await _dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.Users)
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
            var model = new DepotViewModel
            {
                //TODO: ******DESCOMENTAR CUANDO SE CORRIJA EL DROPDOWNLIST EN CASCADA******
                //Departments = _combosHelper.GetComboDepartments(),
                //Municipalities = _combosHelper.GetComboMunicipalities(0),
                //Courts = _combosHelper.GetComboCourts(0),
                //Depots = _combosHelper.GetComboDepots(0),

                //TODO: ******ELIMINAR CUANDO SE CORRIJA EL DROPDOWNLIST EN CASCADA******
                Departments = _combosHelper.GetComboDepartments(),
                Municipalities = _combosHelper.GetComboMunicipalities(),
                Courts = _combosHelper.GetComboCourts(),
            };
            return View(model);
        }

        // POST: Depots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var depot = await ToADepotAsync(model, true);

                try
                {
                    _dataContext.Add(depot);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Almacén Agregado Exitosamente";
                    return RedirectToAction("Index", "Depots");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.ToString());
                }
            }

            return View(model);
        }

        // GET: Depots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depot = await _dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (depot == null)
            {
                return NotFound();
            }
            return View(ToDepotViewModel(depot));
        }

        // POST: Depots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var depot = await ToDepot2Async(model, false);
                try
                {
                    _dataContext.Update(depot);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Almacén Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(model);
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
                try
                {
                    depot.Status = false;
                    depot.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    depot.ModificationDate = DateTime.Now;

                    _dataContext.Update(depot);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageLock"] = "Almacén Bloqueado Exitosamente";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
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
                try
                {
                    depot.Status = true;
                    depot.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    depot.ModificationDate = DateTime.Now;

                    _dataContext.Update(depot);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageUnLock"] = "Almacén Activado Exitosamente";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DepotExists(int id)
        {
            return _dataContext.Depots.Any(e => e.Id == id);
        }

        private async Task<object> ToADepotAsync(DepotViewModel model, bool isNew)
        {
            var depot = new Depot

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                Status = true,
                Observation = model.Observation?.Trim().ToUpper(),

                Court = await _dataContext.Courts.FindAsync(model.CourtId),

                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                Creator = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return depot;
        }

        private DepotViewModel ToDepotViewModel(Depot depot)
        {
            return new DepotViewModel
            {
                Name = depot.Name?.Trim().ToUpper(),
                Status = depot.Status,
                Observation = depot.Observation?.Trim().ToUpper(),

                Court = depot.Court,

                CreationDate = depot.CreationDate,
                Creator = depot.Creator,
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),

                Id = depot.Id,

                CourtId = depot.Court!.Id,

                Courts = _combosHelper.GetComboSuppliers(),
            };
        }

        private async Task<object> ToDepot2Async(DepotViewModel model, bool isNew)
        {
            var depot = new Depot

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                Status = model.Status,
                Observation = model.Observation?.Trim().ToUpper(),

                Court = await _dataContext.Courts.FindAsync(model.CourtId),

                CreationDate = model.CreationDate,
                Creator = model.Creator,
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return depot;
        }
    }
}