using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Enums;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;
using System.Security.Claims;

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
            var departments = _dataContext.Departments.ToList();
            var municipalities = new List<Municipality>();
            var courts = new List<Court>();
            var depots = new List<Depot>();

            departments.Add(new Department()
            {
                Id = 0,
                Name = "--Seleccione una Opción"
            });

            municipalities.Add(new Municipality()
            {
                Id = 0,
                Name = "--Seleccione una Opción"
            });

            courts.Add(new Court()
            {
                Id = 0,
                Name = "--Seleccione una Opción"
            });

            depots.Add(new Depot()
            {
                Id = 0,
                Name = "--Seleccione una Opción"
            });

            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            ViewBag.Municipalities = new SelectList(municipalities, "Id", "Name");
            ViewBag.Courts = new SelectList(courts, "Id", "Name");
            ViewBag.Depots = new SelectList(depots, "Id", "Name");

            var model = new AddUserViewModel
            {
                Departments = _combosHelper.GetComboDepartments(),
                Municipalities = _combosHelper.GetComboMunicipalities(0),
                Courts = _combosHelper.GetComboCourts(0),
                Depots = _combosHelper.GetComboDepots(0),
            };
            return View(model);
        }

        //DROPDOWNLIST IN CASCADE
        public JsonResult GetMunicipalitiesByDepartmentId(int departmentId)
        {
            return Json(_dataContext.Municipalities.Where(u => u.Department!.Id == departmentId).OrderBy(x => x.Name).ToList());
        }

        public JsonResult GetCourtsByMunicipalityId(int municipalityId)
        {
            return Json(_dataContext.Courts.Where(u => u.Municipality!.Id == municipalityId).OrderBy(x => x.Name).ToList());
        }

        public JsonResult GetDepotsByCourtId(int courtId)
        {
            return Json(_dataContext.Depots.Where(u => u.Court!.Id == courtId).OrderBy(x => x.Name).ToList());
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
                DepotId = customer!.User!.DepotId,

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

                    //TODO: tratar de arreglar
                    //customer.User.Depot = await _dataContext.Depots.FindAsync(model.DepotId);
                    //customer.User.DepotId = model.DepotId;
                    //customer.User.Depot = await _dataContext.Depots.FindAsync(model.DepotId);

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

        //************************************probando**********************************************
        // GET: Customers/ResetPassword/5
        public async Task<IActionResult> ResetPasswordAsync(int? id)
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

        // POST: Customers/ResetPassword/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customer = await _dataContext.Customers
        //            .Include(c => c.User)
        //            .FirstOrDefaultAsync(u => u.Id == model.Id);
        //        var token = await _userHelper.GeneratePasswordResetTokenAsync(customer);
        //        if (customer != null)
        //        {
        //            customer.User!.LockoutEnd = null;
        //            customer.User!.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            customer.User.ModificationDate = DateTime.Now;
        //        }
        //    }
        //}

        //public async Task<IActionResult> ResetPassword(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var customer = await _dataContext.Customers
        //        .Include(c => c.User)
        //        .ThenInclude(z => z!.Depot)
        //        .ThenInclude(z => z!.Court)
        //        .ThenInclude(z => z!.Municipality)
        //        .ThenInclude(z => z!.Department)
        //        .FirstOrDefaultAsync(i => i.Id == id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new ResetPasswordViewModel()
        //    {
        //        UserName = customer.User!.UserName,
        //        FullName = customer.User.FullName,
        //        DNI = customer.User.DNI,
        //        Occupation = customer.User.Occupation,
        //        Email = customer.User.Email,
        //        PhoneNumber = customer.User.PhoneNumber,
        //        Observation = customer.User.Observation,
        //        CreationDate = customer.User.CreationDate,
        //        Creator = customer.User.Creator,

        //        Depot = customer.User.Depot,
        //        DepotId = customer!.User!.DepotId,

        //        Departments = _combosHelper.GetComboDepartments(),
        //        Municipalities = _combosHelper.GetComboMunicipalities(),
        //        Courts = _combosHelper.GetComboCourts(),
        //        Depots = _combosHelper.GetComboDepots(),
        //    };
        //    return View(model);
        //}

        // POST: Customers/ResetPassword/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customer = await _dataContext.Customers
        //            .Include(c => c.User)
        //            .FirstOrDefaultAsync(i => i.Id == model.Id);

        //        var user = await _userHelper.GetUserAsync(customer!.User!.UserName!);

        //        if (customer != null)
        //        {
        //            customer.User!.UserName = model.UserName;
        //            customer.User!.LockoutEnd = null;
        //            customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            customer.User.ModificationDate = DateTime.Now;
        //        }

        //        try
        //        {
        //            if (user != null)
        //            {
        //                var token = await _userHelper.GeneratePasswordResetTokenAsync(user);
        //                await _userHelper.ResetPasswordAsync(user, token, newPassword)

        //                var removePassword = await _userHelper.RemovePasswordAsync(user);
        //                if (removePassword.Succeeded)
        //                {
        //                    var addPassword = await _userHelper.AddPasswordAsync(user, model.ResetPassword!);
        //                    if (!addPassword.Succeeded)
        //                    {
        //                        await _userHelper.UpdateUserAsync(customer!.User!);
        //                        await _dataContext.SaveChangesAsync();
        //                        TempData["AlertMessageEdit"] = "Contraseña del Usuario Restablecida Exitosamente";
        //                        return RedirectToAction(nameof(Index));
        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError(string.Empty, addPassword.Errors.FirstOrDefault()!.Description);
        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, removePassword.Errors.FirstOrDefault()!.Description);
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
        //        }
        //    }
        //    return View(model);
        //}

        //******************************************************************************************
        // GET: Customers/ResetPassword/5
        //public async Task<IActionResult> ResetPassword(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var customer = await _dataContext.Customers
        //        .Include(c => c.User)
        //        .ThenInclude(z => z!.Depot)
        //        .ThenInclude(z => z!.Court)
        //        .ThenInclude(z => z!.Municipality)
        //        .ThenInclude(z => z!.Department)
        //        .FirstOrDefaultAsync(i => i.Id == id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    var model = new ResetPasswordViewModel()
        //    {
        //        UserName = customer.User!.UserName,
        //        FullName = customer.User.FullName,
        //        DNI = customer.User.DNI,
        //        Occupation = customer.User.Occupation,
        //        Email = customer.User.Email,
        //        PhoneNumber = customer.User.PhoneNumber,
        //        Observation = customer.User.Observation,
        //        CreationDate = customer.User.CreationDate,
        //        Creator = customer.User.Creator,

        //        Depot = customer.User.Depot,
        //        DepotId = customer!.User!.DepotId,

        //        Departments = _combosHelper.GetComboDepartments(),
        //        Municipalities = _combosHelper.GetComboMunicipalities(),
        //        Courts = _combosHelper.GetComboCourts(),
        //        Depots = _combosHelper.GetComboDepots(),
        //    };
        //    return View(model);
        //}

        // POST: Customers/ResetPassword/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customer = await _dataContext.Customers
        //            .Include(c => c.User)
        //            .FirstOrDefaultAsync(i => i.Id == model.Id);

        //        var user = await _userHelper.GetUserAsync(customer!.User!.UserName!);

        //        if (customer != null)
        //        {
        //            customer.User!.UserName = model.UserName;
        //            customer.User!.LockoutEnd = null;
        //            customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            customer.User.ModificationDate = DateTime.Now;
        //        }

        //        try
        //        {
        //            if (user != null)
        //            {
        //                var removePassword = await _userHelper.RemovePasswordAsync(user);
        //                if (removePassword.Succeeded)
        //                {
        //                    var addPassword = await _userHelper.AddPasswordAsync(user, model.ResetPassword!);
        //                    if (!addPassword.Succeeded)
        //                    {
        //                        await _userHelper.UpdateUserAsync(customer!.User!);
        //                        await _dataContext.SaveChangesAsync();
        //                        TempData["AlertMessageEdit"] = "Contraseña del Usuario Restablecida Exitosamente";
        //                        return RedirectToAction(nameof(Index));
        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError(string.Empty, addPassword.Errors.FirstOrDefault()!.Description);
        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, removePassword.Errors.FirstOrDefault()!.Description);
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
        //        }
        //    }
        //    return View(model);
        //}

        //*****************************************************************************************************************
        //CASI PERO NO

        //// POST: Customers/ResetPassword/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var customer = await _dataContext.Customers
        //            .Include(c => c.User)
        //            .FirstOrDefaultAsync(i => i.Id == model.Id);

        //        if (customer != null)
        //        {
        //            customer.User!.UserName = model.UserName;
        //            customer.User!.LockoutEnd = null;
        //            customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            customer.User.ModificationDate = DateTime.Now;
        //        }

        //        try
        //        {
        //            var user = await _userHelper.GetUserAsync(customer!.User!.UserName!);
        //            if (user != null)
        //            {
        //                var removePassword = await _userHelper.RemovePasswordAsync(user);
        //                if (removePassword.Succeeded)
        //                {
        //                    var addPassword = await _userHelper.AddPasswordAsync(user, model.NewPassword!);
        //                    if (!addPassword.Succeeded)
        //                    {
        //                        await _userHelper.UpdateUserAsync(customer!.User!);
        //                        await _dataContext.SaveChangesAsync();
        //                        TempData["AlertMessageEdit"] = "Contraseña del Usuario Restablecida Exitosamente";
        //                        return RedirectToAction(nameof(Index));
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    return View(model);
        //}

        //***************************************************************************************************************

        //var user = await _userHelper.GetUserAsync(customer!.User!.UserName!);
        //var removePassword = await _userHelper.RemovePasswordAsync(user);
        //if (removePassword.Succeeded)
        //{
        //    var addPassword = await _userHelper.AddPasswordAsync(user, model.NewPassword!);
        //    if (addPassword.Succeeded)
        //    {
        //        await _userHelper.UpdateUserAsync(customer!.User!);
        //        await _dataContext.SaveChangesAsync();
        //        TempData["AlertMessageEdit"] = "Contraseña del Usuario Restablecida Exitosamente";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        ModelState.AddModelError(string.Empty, removePassword.Errors.FirstOrDefault()!.Description);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //if (ModelState.IsValid)
        //{
        //    var customer = await _dataContext.Customers
        //        .Include(c => c.User)
        //        .ThenInclude(z => z!.Depot)
        //        .ThenInclude(z => z!.Court)
        //        .ThenInclude(z => z!.Municipality)
        //        .ThenInclude(z => z!.Department)
        //        .FirstOrDefaultAsync(i => i.Id == model.Id);

        //    if (customer != null)
        //    {
        //        customer.User!.UserName = model.UserName;
        //        customer.User!.FullName = model.FullName;
        //        customer.User.DNI = model.DNI;
        //        customer.User.Occupation = model.Occupation;
        //        customer.User.Email = model.Email;
        //        customer.User.PhoneNumber = model.PhoneNumber;
        //        customer.User.Observation = model.Observation;
        //        customer.User.Depot = await _dataContext.Depots.FindAsync(model.DepotId);

        //        customer.User.Creator = model.Creator;
        //        customer.User.CreationDate = model.CreationDate;

        //        customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        customer.User.ModificationDate = DateTime.Now;

        //        try
        //        {
        //            var user = await _userHelper.GetUserAsync(customer.User.UserName!);
        //            var removePassword = await _userHelper.RemovePasswordAsync(user);
        //            if (removePassword.Succeeded)
        //            {
        //                var addPassword = await _userHelper.AddPasswordAsync(user, model.NewPassword!);
        //                if (addPassword.Succeeded)
        //                {
        //                    await _dataContext.SaveChangesAsync();
        //                    TempData["AlertMessageEdit"] = "Contraseña del Usuario Restablecida Exitosamente";
        //                    return View("Index");
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError(string.Empty, removePassword.Errors.FirstOrDefault()!.Description);
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //return View(model);
        //}

        //var removePassword = _userHelper.RemovePasswordAsync(model.Id);
        //if (removePassword.Succeeded)
        //{
        //    var addPassword = _userHelper.AddPasswordAsync(model.Id, model.Password);
        //    if (addPassword.Succeeded)
        //    {
        //        return View("Index");
        //    }
        //}

        //if (removePassword != null)
        //{
        //    var addPassword = _userHelper.AddPasswordAsync(model.Id, model.NewPassword);
        //    if (addPassword.IsCompleted)
        //    {
        //        return View("Index");
        //    }
        //}
        //if (removePassword.Succeeded)
        //{
        //    var addPassword = _userHelper.AddPasswordAsync(model.Id, model.NewPassword);
        //    if (addPassword.IsCompleted)
        //    {
        //        return View("Index");
        //    }
        //}

        //var removePassword = _userHelper.RemovePasswordAsync(model.Id);
        //if (removePassword.)
        //{
        //    var addPassword = _userHelperAddPasswordAsync(model.Id, model.NewPassword);
        //    if (addPassword.IsCompleted)
        //    {
        //        return View("Index");
        //    }
        //}
        //return View(model);

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