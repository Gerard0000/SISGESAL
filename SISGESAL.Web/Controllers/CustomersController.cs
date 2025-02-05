using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Data;
using SISGESAL.web.Enums;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CustomersController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper) : Controller
    {
        private readonly DataContext _dataContext = context;
        private readonly IUserHelper _userHelper = userHelper;
        private readonly ICombosHelper _combosHelper = combosHelper;

        // GET: Managers LE QUITAMOS ASYNC TASK<>
        public IActionResult Index()
        {
            //USUARIOS ACTIVOS
            ViewBag.Indexcount = _dataContext.Customers.Count();
            //USUARIOS BLOQUEADOS
            ViewBag.Indexcount2 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.LockoutEnd == null).Count();
            //TOTAL DE USUARIOS
            ViewBag.Indexcount3 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.LockoutEnd > DateTime.Now).Count();
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

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department));
        }

        //USUARIOS ACTIVOS CON ALMACÉN ACTIVO
        public IActionResult ActiveUserWithDepot()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == true)));
        }

        //USUARIOS BLOQUEADOS CON ALMACÉN ACTIVO
        public IActionResult LockedUserWithDepot()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == true)).Count();

            return View(_dataContext.Customers

                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == true)));
        }

        //USUARIOS CON ALMACÉN ACTIVO
        public IActionResult UserWithDepot()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.Depot!.Status == true)).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot != null) && (m.User!.Depot!.Status == true)));
        }

        //USUARIOS ACTIVOS CON ALMACÉN BLOQUEADO
        public IActionResult ActiveUserWithDepotLock()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == false)).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null) && (m.User!.Depot!.Status == false)));
        }

        //USUARIOS BLOQUEADOS CON ALMACÉN BLOQUEADO
        public IActionResult LockedUserWithDepotLock()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == false)).Count();

            return View(_dataContext.Customers

                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null) && (m.User!.Depot!.Status == false)));
        }

        //USUARIOS CON ALMACÉN BLOQUEADO
        public IActionResult UserWithDepotLock()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.Depot!.Status == false)).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot != null) && (m.User!.Depot!.Status == false)));
        }

        //USUARIOS ACTIVOS SIN ALMACÉN
        public IActionResult ActiveUserWithNoDepot()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.User.LockoutEnd == null)).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot == null) && (m.User.LockoutEnd == null)));
        }

        //USUARIOS BLOQUEADOS CON ALMACÉN
        public IActionResult LockedUserWithNoDepot()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.User!.LockoutEnd != null)).Count();

            return View(_dataContext.Customers

                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => (m.User!.Depot == null) && (m.User!.LockoutEnd != null)));
        }

        //USUARIOS CON ALMACÉN
        public IActionResult UserWithNoDepot()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null)).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => m.User!.Depot == null));
        }

        //USUARIOS ACTIVOS
        public IActionResult ActiveUser()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.LockoutEnd == null).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => m.User!.LockoutEnd == null));
        }

        //USUARIOS BLOQUEADOS
        public IActionResult LockedUser()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.LockoutEnd != null).Count();

            return View(_dataContext.Customers

                .Include(m => m.User!)
                .ThenInclude(c => c.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .Where(m => m.User!.LockoutEnd != null));
        }

        //TOTAL DE USUARIOS
        public IActionResult TotalUser()
        {
            ViewBag.Indexcount = _dataContext.Customers.Include(x => x.User).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
            .ThenInclude(c => c.Depot)
            .ThenInclude(z => z!.Court)
            .ThenInclude(z => z!.Municipality)
            .ThenInclude(z => z!.Department));
        }

        //*******************************************INTENTAR DROPDOWNLIST EN CASCADA****************************
        //[HttpGet]
        //public IActionResult GetDepartments()
        //{
        //    var countries = _dataContext.Departments.ToList();
        //    return Json(new SelectList(countries, "Id", "Name"));
        //}

        //[HttpGet]
        //public IActionResult GetMunicipalities(int Id)
        //{
        //    var municipalities = _dataContext.Municipalities.Where(x => x.Department!.Id == Id).ToList();
        //    return Json(new SelectList(municipalities, "Id", "Name"));
        //}

        //[HttpGet]
        //public IActionResult GetCourts(int Id)
        //{
        //    var courts = _dataContext.Courts.Where(x => x.Municipality!.Id == Id).ToList();
        //    return Json(new SelectList(courts, "Id", "Name"));
        //}
        //********************************************************************************************************

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _dataContext.Customers
                .Include(c => c.User!)
                .ThenInclude(z => z.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            var model = new AddUserViewModel
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
                Depots = _combosHelper.GetComboDepots(),
            };
            return View(model);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (_dataContext.Users.Any(x => x.UserName == model.UserName))
            {
                ViewBag.DuplicateMessage1 = $"El Nombre de Usuario ya esta asignado a otro usuario";
            }
            if (_dataContext.Users.Any(x => x.DNI == model.DNI))
            {
                ViewBag.DuplicateMessage2 = $"El DNI ya esta asignado a otro usuario";
            }
            if (_dataContext.Users.Any(x => x.Email == model.Email))
            {
                ViewBag.DuplicateMessage3 = $"El Correo Electrónico ya esta asignado a otro usuario";
            }
            if (_dataContext.Users.Any(x => x.Depot!.Id == model.DepotId))
            {
                ViewBag.DuplicateMessage4 = $"El Almacén ya esta asignado a otro usuario";
            }
            else if (ModelState.IsValid)
            {
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

                    UserType = UserType.Customer,
                };

                var response = await _userHelper.AddUserAsync(user, model.Password!);
                if (response.Succeeded)
                {
                    try
                    {
                        var userInDb = await _userHelper.GetUserAsync(model.UserName!);
                        await _userHelper.AddUserToRoleAsync(userInDb, UserType.Customer);
                        var customer = new Customer
                        {
                            User = userInDb
                        };

                        _dataContext.Customers.Add(customer);
                        await _dataContext.SaveChangesAsync();
                        TempData["AlertMessageCreate"] = "Usuario Agregado Exitosamente";
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

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _dataContext.Customers
                .Include(c => c.User!)
                .ThenInclude(z => z.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(c => c.Id == id.Value);

            if (customer == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                UserName = customer.User!.UserName,
                FullName = customer.User.FullName,
                DNI = customer.User.DNI,
                Occupation = customer.User.Occupation,
                Email = customer.User.Email,
                PhoneNumber = customer.User.PhoneNumber,
                Observation = customer.User.Observation,
                CreationDate = customer.User.CreationDate,
                Creator = customer.User.Creator,

                Depot = customer.User.Depot,
                DepotId = customer!.User!.Depot!.Id!,

                Departments = _combosHelper.GetComboDepartments(),
                Municipalities = _combosHelper.GetComboMunicipalities(),
                Courts = _combosHelper.GetComboCourts(),
                Depots = _combosHelper.GetComboDepots(),
            };

            return View(model);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _dataContext.Customers
                .Include(c => c.User)
                .ThenInclude(z => z!.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

                if (customer != null)
                {
                    customer.User!.UserName = model.UserName;
                    customer.User!.FullName = model.FullName?.Trim().ToUpper();
                    customer.User.DNI = model.DNI?.Trim();
                    customer.User.Occupation = model.Occupation?.Trim().ToUpper();
                    customer.User.Email = model.Email?.Trim().ToLower();
                    customer.User.PhoneNumber = model.PhoneNumber?.Trim();
                    customer.User.Observation = model.Observation?.Trim().ToUpper();

                    customer.User.Depot = await _dataContext.Depots.FindAsync(model.DepotId);

                    customer.User.Creator = model.Creator;
                    customer.User.CreationDate = model.CreationDate;

                    customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    customer.User.ModificationDate = DateTime.Now;
                }
                try
                {
                    await _userHelper.UpdateUserAsync(customer!.User!);
                    _dataContext.Update(customer);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Usuario Actualizado Exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer!.Id))
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

        // GET: Customers/AsignDepot/5
        public async Task<IActionResult> AsignDepot(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _dataContext.Customers
                .Include(c => c.User!)
                .ThenInclude(z => z.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(c => c.Id == id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                UserName = customer.User!.UserName,
                FullName = customer.User.FullName,
                DNI = customer.User.DNI,
                Occupation = customer.User.Occupation,
                Email = customer.User.Email,
                PhoneNumber = customer.User.PhoneNumber,
                Observation = customer.User.Observation,
                CreationDate = customer.User.CreationDate,
                Creator = customer.User.Creator,

                DepotId = customer.User.Depot!.Id,

                Departments = _combosHelper.GetComboDepartments(),
                Municipalities = _combosHelper.GetComboMunicipalities(),
                Courts = _combosHelper.GetComboCourts(),
                Depots = _combosHelper.GetComboDepots(),
            };
            return View(model);
        }

        // POST: Customers/AsignDepot/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AsignDepot(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _dataContext.Customers
                .Include(c => c.User)
                .ThenInclude(z => z!.Depot)
                .ThenInclude(z => z!.Court)
                .ThenInclude(z => z!.Municipality)
                .ThenInclude(z => z!.Department)
                .FirstOrDefaultAsync(c => c.Id == model.Id);

                if (customer != null)
                {
                    customer.User!.UserName = model.UserName;
                    customer.User!.FullName = model.FullName;
                    customer.User.DNI = model.DNI;
                    customer.User.Occupation = model.Occupation;
                    customer.User.Email = model.Email;
                    customer.User.PhoneNumber = model.PhoneNumber;
                    customer.User.Observation = model.Observation;

                    customer.User.Depot = await _dataContext.Depots.FindAsync(model.DepotId);

                    customer.User.Creator = model.Creator;
                    customer.User.CreationDate = model.CreationDate;

                    customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    customer.User.ModificationDate = DateTime.Now;
                }
                try
                {
                    await _userHelper.UpdateUserAsync(customer!.User!);
                    _dataContext.Update(customer);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Almacén Asignado Exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer!.Id))
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

        // GET: Customers/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _dataContext.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var customer = await _dataContext.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (customer != null)
            {
                customer.User!.LockoutEnd = DateTime.Now.AddYears(1000);
                customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.User.ModificationDate = DateTime.Now;
            }

            try
            {
                await _userHelper.UpdateUserAsync(customer!.User!);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageLock"] = "Usuario Bloqueado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Customers/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = await _dataContext.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var customer = await _dataContext.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (customer != null)
            {
                customer.User!.LockoutEnd = null;
                customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.User.ModificationDate = DateTime.Now;
            }

            try
            {
                await _userHelper.UpdateUserAsync(customer!.User!);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageUnLock"] = "Usuario Activado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CustomerExists(int id)
        {
            return _dataContext.Customers.Any(e => e.Id == id);
        }
    }
}