using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Data;
using SISGESAL.web.Enums;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagersController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper) : Controller
    {
        private readonly DataContext _dataContext = context;
        private readonly IUserHelper _userHelper = userHelper;
        private readonly ICombosHelper _combosHelper = combosHelper;

        // GET: Managers LE QUITAMOS ASYNC TASK<> TODOS LOS USUARIOS
        public IActionResult Index()
        {
            ViewBag.Indexcount = _dataContext.Managers.Count();
            ViewBag.Indexcount2 = _dataContext.Managers.Include(x => x.User).Where(m => m.User!.LockoutEnd == null).Count();
            ViewBag.Indexcount3 = _dataContext.Managers.Include(x => x.User).Where(m => m.User!.LockoutEnd > DateTime.Now).Count();

            return View(_dataContext.Managers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department));
        }

        //*******************************INTENTO DE HACER DROPDOWNLIST EN CASCADA*******************
        //*******************************REVISAR PRIMERO EL CUSTOMER*******************
        //[HttpGet]
        //public async Task<IActionResult> GetDepartments()
        //{
        //    var departments = _dataContext.Departments.ToList();
        //    return Json(new SelectList(departments, "Id", "Name"));
        //}

        //Metodo Json para dropdonlist in cascade
        //public async Task<JsonResult> GetCourtsAsync(int mulicipalityId)
        //{
        //    var country = await _
        //}

        //public async Task<JsonResult> GetMunicipalities(int countryId)
        //{
        //    var country =
        //}

        //[HttpGet]
        //public IActionResult GetMunicipalities(int? departmentId)
        //{
        //    var municipalities = _dataContext.Municipalities.Where(x => x.Department.Id == departmentId).ToList();
        //    return Json(new SelectList(municipalities, "Id", "Name"));
        //}

        //[HttpGet]
        //public IActionResult GetCourts(int? municipalityId)
        //{
        //    var courts = _dataContext.Courts.Where(x => x.Municipality.Id == municipalityId).ToList();
        //    return Json(new SelectList(courts, "Id", "Name"));
        //}

        //public async Task<JsonResult> GetMunicipalitiesAsync(int departmentId)
        //{
        //    var department = await _combosHelper.GetDepartmentWithMunicipalityAsync(departmentId);
        //    //return Json(department!.Municipalities!.OrderBy(c => c.Name));
        //    return Json(new SelectList(department.Municipalities, "Id", "Name"));
        //}

        //public JsonResult GetMunicipalities(int? departmentId)
        //{
        //    var municipalities = _dataContext.Municipalities.Where(x => x.Department.Id == departmentId).ToList();
        //    return Json(new SelectList(municipalities, "Id", "Name"));
        //}

        //public JsonResult GetCourts(int municipalityId)
        //{
        //    var courts = _dataContext.Courts.Where(x => x!.Municipality!.Id == municipalityId).ToList();
        //    return Json(new SelectList(courts, "Id", "Name"));
        //}

        //**************************************************************************

        // GET: Managers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _dataContext.Managers
                .Include(c => c.User!)
                .ThenInclude(z => z.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        // GET: Managers/Create
        public IActionResult Create()
        {
            var model = new AddUserViewModel
            {
                //TODO: DESCOMENTAR CUANDO SE CORRIJA EL DROPDOWNLIST EN CASCADA
                //Departments = _combosHelper.GetComboDepartments(),
                //Municipalities = _combosHelper.GetComboMunicipalities(0),
                //Courts = _combosHelper.GetComboCourts(0),
                //Depots = _combosHelper.GetComboDepots(0),

                //TODO: ELIMINAR CUANDO SE CORRIJA EL DROPDOWNLIST EN CASCADA
                Departments = _combosHelper.GetComboDepartments(),
                Municipalities = _combosHelper.GetComboMunicipalities(),
                Courts = _combosHelper.GetComboCourts(),
                Depots = _combosHelper.GetComboDepots(),
            };
            return View(model);
        }

        // POST: Managers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //UTILIZAMOS METODO PRIVADO QUE ESTA ABAJO PARA LA CREACIÓN DE LA NOTIFICACIÓN
                //var notification = await ToManagerAsync(model, true);

                //var municipality = await _combosHelper.GetMunicipalityAsync(model.DepartmentId);
                //var court = await _combosHelper.GetComboCourts(model.CourtId);

                //var municipality = await _combosHelper.GetMunicipalityAsync(model.MunicipalityId);
                //var court = await _combosHelper.GetCourtAsync(model.CourtId);

                //var municipality = await _combosHelper.GetMunicipalityAsync(model.MunicipalityId);
                //var court = await _combosHelper.GetCourtAsync(model.CourtId);

                var user = new User
                {
                    UserName = model.UserName?.Trim().ToLower(),
                    FullName = model.FullName?.Trim().ToUpper(),
                    DNI = model.DNI?.Trim(),
                    Occupation = model.Occupation?.Trim().ToUpper(),
                    Email = model.Email?.Trim().ToLower(),
                    PhoneNumber = model.PhoneNumber?.Trim(),
                    Observation = model.Observation?.Trim().ToUpper(),

                    Depot = await _dataContext.Depots.FindAsync(model.DepotId),

                    CreationDate = DateTime.Now,
                    ModificationDate = DateTime.Now,
                    Creator = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),

                    UserType = UserType.Manager,
                };

                var response = await _userHelper.AddUserAsync(user, model!.Password!);

                if (response.Succeeded)
                {
                    try
                    {
                        var userInDb = await _userHelper.GetUserAsync(model!.UserName!);
                        await _userHelper.AddUserToRoleAsync(userInDb, UserType.Manager);
                        var manager = new Manager
                        {
                            User = userInDb
                        };

                        _dataContext.Managers.Add(manager);
                        await _dataContext.SaveChangesAsync();
                        TempData["AlertMessageCreate"] = "Administrador Agregado Exitosamente";
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception)
                    {
                        ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                    }
                }
                //ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
            }

            model.Departments = _combosHelper.GetComboDepartments();
            model.Municipalities = _combosHelper.GetComboMunicipalities();
            model.Courts = _combosHelper.GetComboCourts();
            model.Depots = _combosHelper.GetComboDepots();

            return View(model);
        }

        // GET: Managers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manager = await _dataContext.Managers
                .Include(c => c.User!)
                .ThenInclude(z => z.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(c => c.Id == id.Value);
            if (manager == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                UserName = manager.User!.UserName,
                FullName = manager.User.FullName,
                DNI = manager.User.DNI,
                Occupation = manager.User.Occupation,
                Email = manager.User.Email,
                PhoneNumber = manager.User.PhoneNumber,
                Observation = manager.User.Observation,
                CreationDate = manager.User.CreationDate,
                Creator = manager.User.Creator,

                Depot = manager.User.Depot,

                Departments = _combosHelper.GetComboDepartments(),
                Municipalities = _combosHelper.GetComboMunicipalities(),
                Courts = _combosHelper.GetComboCourts(),
                Depots = _combosHelper.GetComboDepots(),
            };
            return View(model);
        }

        // POST: Managers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = await _dataContext.Managers
                .Include(c => c.User)
                .ThenInclude(z => z!.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

                if (manager != null)
                {
                    manager.User!.UserName = model.UserName;

                    manager!.User!.FullName = model.FullName?.Trim().ToUpper();
                    manager.User.DNI = model.DNI?.Trim();
                    manager.User.Occupation = model.Occupation?.Trim().ToUpper();
                    manager.User.Email = model.Email?.Trim().ToLower();
                    manager.User.PhoneNumber = model.PhoneNumber?.Trim();
                    manager.User.Observation = model.Observation?.Trim().ToUpper();

                    manager.User.Depot = await _dataContext.Depots.FindAsync(model.DepotId);

                    manager.User.Creator = model.Creator;
                    manager.User.CreationDate = model.CreationDate;

                    manager.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    manager.User.ModificationDate = DateTime.Now;
                }
                try
                {
                    await _userHelper.UpdateUserAsync(manager!.User!);
                    _dataContext.Update(manager);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Administrador Actualizado Exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager!.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            model.Departments = _combosHelper.GetComboDepartments();
            model.Municipalities = _combosHelper.GetComboMunicipalities();
            model.Courts = _combosHelper.GetComboCourts();
            model.Depots = _combosHelper.GetComboDepots();

            return View(model);
        }

        // GET: Managers/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manager = await _dataContext.Managers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        // POST: Managers/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var manager = await _dataContext.Managers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (manager != null)
            {
                manager.User!.LockoutEnd = DateTime.Now.AddYears(1000);
                manager.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                manager.User.ModificationDate = DateTime.Now;
            }

            try
            {
                await _userHelper.UpdateUserAsync(manager!.User!);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageLock"] = "Administrador Bloqueado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Managers/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var manager = await _dataContext.Managers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (manager == null)
            {
                return NotFound();
            }
            return View(manager);
        }

        // POST: Managers/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var manager = await _dataContext.Managers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (manager != null)
            {
                manager.User!.LockoutEnd = null;
                manager.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                manager.User.ModificationDate = DateTime.Now;
            }
            try
            {
                await _userHelper.UpdateUserAsync(manager!.User!);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageUnLock"] = "Administrador Activado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool ManagerExists(int id)
        {
            return _dataContext.Managers.Any(e => e.Id == id);
        }

        //private async Task<User> ToManagerAsync(AddUserViewModel model, bool isNew)
        //{
        //    var user = new AddUserViewModel

        //    {
        //        Id = isNew ? 0 : model.Id,

        //        UserName = model.UserName?.Trim().ToLower(),
        //        FullName = model.FullName?.Trim().ToUpper(),
        //        DNI = model.DNI?.Trim(),
        //        Occupation = model.Occupation?.Trim().ToUpper(),
        //        Email = model.Email?.Trim().ToLower(),
        //        PhoneNumber = model.PhoneNumber?.Trim(),
        //        Observation = model.Observation?.Trim().ToUpper(),

        //        Court = await _dataContext.Courts.FindAsync(model.CourtId),

        //        CreationDate = DateTime.Now,
        //        ModificationDate = DateTime.Now,
        //        Creator = User.FindFirstValue(ClaimTypes.NameIdentifier),
        //        Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
        //    };

        //    return user;
        //}
    }
}