﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Data;
using SISGESAL.web.Enums;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CustomersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public CustomersController(DataContext context, IUserHelper userHelper, ICombosHelper combosHelper)
        {
            _dataContext = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
        }

        // GET: Managers LE QUITAMOS ASYNC TASK<>
        public IActionResult Index()
        {
            ViewBag.Indexcount = _dataContext.Customers.Count();
            ViewBag.Indexcount2 = _dataContext.Customers.Include(x => x.User).Where(m => m.User.LockoutEnd == null).Count();
            ViewBag.Indexcount3 = _dataContext.Customers.Include(x => x.User).Where(m => m.User.LockoutEnd > DateTime.Now).Count();

            return View(_dataContext.Customers
                .Include(m => m.User!)
                .ThenInclude(c => c.Court));
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
                .ThenInclude(z => z.Court)
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
                Courts = _combosHelper.GetComboCourts(),
            };

            return View(model);
            //return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
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

                    Court = await _dataContext.Courts.FindAsync(model.CourtId),

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
                ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
            }

            model.Courts = _combosHelper.GetComboCourts();

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
                .ThenInclude(z => z.Court)
                .FirstOrDefaultAsync(c => c.Id == id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                UserName = customer?.User?.UserName,
                FullName = customer?.User?.FullName,
                DNI = customer?.User?.DNI,
                Occupation = customer?.User?.Occupation,
                Email = customer?.User?.Email,
                PhoneNumber = customer?.User?.PhoneNumber,
                Observation = customer?.User?.Observation,
                CreationDate = customer.User.CreationDate,
                Creator = customer.User.Creator,

                Court = customer?.User?.Court,

                //CourtId = manager.User.Court.Id,

                Courts = _combosHelper.GetComboCourts(),
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
                .FirstOrDefaultAsync(c => c.Id == model.Id);

                if (customer != null)
                {
                    customer.User.FullName = model.FullName?.Trim().ToUpper();
                    customer.User.DNI = model.DNI?.Trim();
                    customer.User.Occupation = model.Occupation?.Trim().ToUpper();
                    customer.User.Email = model.Email?.Trim().ToLower();
                    customer.User.PhoneNumber = model.PhoneNumber?.Trim();
                    customer.User.Observation = model.Observation?.Trim().ToUpper();

                    customer.User.Court = await _dataContext.Courts.FindAsync(model.CourtId);

                    customer.User.Creator = model.Creator;
                    customer.User.CreationDate = model.CreationDate;

                    customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    customer.User.ModificationDate = DateTime.Now;
                }
                try
                {
                    await _userHelper.UpdateUserAsync(customer.User);
                    _dataContext.Update(customer);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Usuario Actualizado Exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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

            model.Courts = _combosHelper.GetComboCourts();

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
                customer.User.LockoutEnd = DateTime.Now.AddYears(1000);
                customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.User.ModificationDate = DateTime.Now;
            }

            try
            {
                await _userHelper.UpdateUserAsync(customer.User);
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
                customer.User.LockoutEnd = null;
                customer.User.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                customer.User.ModificationDate = DateTime.Now;
            }

            try
            {
                await _userHelper.UpdateUserAsync(customer.User);
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