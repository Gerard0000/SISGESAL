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
    public class SuppliersController : Controller
    {
        private readonly DataContext _dataContext;

        public SuppliersController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Suppliers.Count();
            ViewBag.Indexcount2 = _dataContext.Suppliers.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.Suppliers.Where(m => m.Status == false).Count();
            return View(await _dataContext.Suppliers.ToListAsync());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _dataContext.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] Supplier supplier)
        {
            //VALOR DUPLICADO
            if (_dataContext.Departments.Any(x => x.Name == supplier.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{supplier.Name}' ya se esta usando";
            }
            else
            if (ModelState.IsValid)
            {
                supplier.Name = supplier.Name?.ToUpper().Trim();
                supplier.Status = true;
                supplier.Observation = supplier.Observation?.ToUpper().Trim();
                supplier.CreationDate = DateTime.Now;
                supplier.ModificationDate = DateTime.Now;
                supplier.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                supplier.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    _dataContext.Add(supplier);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Proveedor Agregado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _dataContext.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    supplier.Name = supplier.Name?.ToUpper().Trim();
                    supplier.Observation = supplier.Observation?.ToUpper().Trim();
                    supplier.Status = supplier.Status;
                    supplier.CreationDate = supplier.CreationDate;
                    supplier.Creator = supplier.Creator;
                    supplier.ModificationDate = DateTime.Now;
                    supplier.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dataContext.Update(supplier);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Proveedor Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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
            return View(supplier);
            }

        // GET: Suppliers/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var supplier = await _dataContext.Suppliers
                .FirstOrDefaultAsync(u => u.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var supplier = await _dataContext.Suppliers
                .FirstOrDefaultAsync(u => u.Id == id);
            if (supplier != null)
            {
                supplier.Status = false;
                supplier.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                supplier.ModificationDate = DateTime.Now;
            }

            try
            {
                _dataContext.Update(supplier);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageLock"] = "Proveedor Bloqueado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Suppliers/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var supplier = await _dataContext.Suppliers
                .FirstOrDefaultAsync(u => u.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var supplier = await _dataContext.Suppliers
                .FirstOrDefaultAsync(u => u.Id == id);
            if (supplier != null)
            {
                supplier.Status = true;
                supplier.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                supplier.ModificationDate = DateTime.Now;
            }
            try
            {
                _dataContext.Update(supplier);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageUnLock"] = "Proveedor Activado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TradeMarks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suppliers = await _dataContext.Suppliers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suppliers == null)
            {
                return NotFound();
            }

            return View(suppliers);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _dataContext.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _dataContext.Suppliers.Remove(supplier);
            }

            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageDelete"] = "Marca de Artículo Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con algún otra instancia de la base de datos.";
            }
            return RedirectToAction(nameof(Index));
        }
        private bool SupplierExists(int id)
        {
            return _dataContext.Suppliers.Any(e => e.Id == id);
        }
    }
}
