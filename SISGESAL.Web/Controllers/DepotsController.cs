using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DepotsController(DataContext context, ICombosHelper combosHelper) : Controller
    {
        private readonly DataContext _dataContext = context;
        private readonly ICombosHelper _combosHelper = combosHelper;

        // GET: Depots
        public async Task<IActionResult> Index()
        {
            //ALMACENES ACTIVOS CON USUARIOS ACTIVOS
            ViewBag.Indexcount1 = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)).Count();
            //USUARIOS BLOQUEADOS CON ALMACÉN ACTIVO
            ViewBag.Indexcount2 = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == true)).Count();
            //ALMACÉN ACTIVO SIN USUARIO
            ViewBag.Indexcount3 = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot! == null) && (m.Status == true)).Count();
            //ALMACENES ACTIVOS
            ViewBag.Indexcount4 = _dataContext.Depots.Where(x => x.Status == true).Count();
            //USUARIOS ACTIVOS CON ALMACÉN BLOQUEADO
            ViewBag.Indexcount5 = _dataContext.Depots.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd == null)).Count();
            //USUARIOS BLOQUEADOS CON ALMACÉN BLOQUEADO
            ViewBag.Indexcount6 = _dataContext.Depots.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd > DateTime.Now)).Count();
            //ALMACÉN BLOQUEADO SIN USUARIO
            ViewBag.Indexcount7 = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.Status == false)).Count();
            //ALMACENES BLOQUEADOS
            ViewBag.Indexcount8 = _dataContext.Depots.Where(x => x.Status == false).Count();
            //TOTAL DE ALMACENES
            ViewBag.Indexcount9 = _dataContext.Depots.Count();

            return View(await _dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .ToListAsync());
        }

        //USUARIOS ACTIVOS CON ALMACÉN ACTIVO
        public IActionResult ActiveUserWithDepotActive()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)).Count();

            //return View(_dataContext.Customers
            //    .Include(m => m.User!)
            //    .ThenInclude(c => c.Depot)
            //    .ThenInclude(z => z!.Court)
            //    .ThenInclude(z => z!.Municipality)
            //    .ThenInclude(z => z!.Department)
            //    .Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)));

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)));
        }

        //USUARIOS BLOQUEADOS CON ALMACÉN ACTIVO
        public IActionResult LockedUserWithDepotActive()
        {
            ViewBag.Indexcount = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == true)).Count();

            //return View(_dataContext.Customers
            //    .Include(m => m.User!)
            //    .ThenInclude(c => c.Depot)
            //    .ThenInclude(z => z!.Court)
            //    .ThenInclude(z => z!.Municipality)
            //    .ThenInclude(z => z!.Department)
            //    .Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == true)));
            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd != null) && (m.User!.Depot!.Status == true)));

        }

        //ALMACENES ACTIVOS SIN USUARIO
        public IActionResult DepotActiveWithNoUser()
        {
            ViewBag.Indexcount = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot! == null) && (m.Status == true)).Count();

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(m => (m.User!.Depot! == null) && (m.Status == true)));
        }

        //ALMACENES ACTIVOS
        public IActionResult DepotActive()
        {
            ViewBag.Indexcount = _dataContext.Depots.Where(x => x.Status == true).Count();

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(x => x.Status == true));
        }

        //USUARIOS ACTIVOS CON ALMACÉN BLOQUEADO
        public IActionResult ActiveUserWithDepotLock()
        {
            ViewBag.Indexcount = _dataContext.Depots.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd == null)).Count();

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd == null)));
        }

        //USUARIOS BLOQUEADOS CON ALMACÉN BLOQUEADO
        public IActionResult LockedUserWithDepotLock()
        {
            ViewBag.Indexcount = _dataContext.Depots.Include(u => u.User).ThenInclude(d => d!.Depot).Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd > DateTime.Now)).Count();

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(x => (x.User!.Depot != null) && (x.User!.Depot!.Status == false) && (x.User.LockoutEnd > DateTime.Now)));
        }

        //ALMACÉN BLOQUEADO SIN USUARIOS
        public IActionResult DepotLockWithNoUser()
        {
            ViewBag.Indexcount = _dataContext.Depots.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.Status == false)).Count();


            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(m => (m.User!.Depot == null) && (m.Status == false)));
        }

        //ALMACENES BLOQUEADOS
        public IActionResult DepotLock()
        {
            ViewBag.Indexcount = _dataContext.Depots.Where(x => x.Status == false).Count();

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .Where(x => x.Status == false));
        }

        //TOTAL DE ALMACENES
        public IActionResult TotalDepot()
        {
            ViewBag.Indexcount = _dataContext.Depots.Count();

            return View(_dataContext.Depots
                .Include(c => c.Court)
                .ThenInclude(m => m!.Municipality)
                .ThenInclude(d => d!.Department)
                .Include(u => u.User)
                .ToList());
        }

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
                .Include(u => u.User)
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

            model.Departments = _combosHelper.GetComboDepartments();
            model.Municipalities = _combosHelper.GetComboMunicipalities();
            model.Courts = _combosHelper.GetComboCourts();

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
                .Include(u => u.User)
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

            model.Departments = _combosHelper.GetComboDepartments();
            model.Municipalities = _combosHelper.GetComboMunicipalities();
            model.Courts = _combosHelper.GetComboCourts();

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