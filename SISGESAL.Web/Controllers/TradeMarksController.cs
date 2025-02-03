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
    public class TradeMarksController(DataContext context) : Controller
    {
        private readonly DataContext _dataContext = context;

        // GET: TradeMarks
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.TradeMarks.Count();
            ViewBag.Indexcount2 = _dataContext.TradeMarks.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.TradeMarks.Where(m => m.Status == false).Count();
            return View(await _dataContext.TradeMarks
                .Include(x => x.Articles)
                .ToListAsync());
        }

        // GET: TradeMarks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeMark = await _dataContext.TradeMarks
                .Include(x => x.Articles)
                .ThenInclude(x => x.KindofArticle)
                .Include(x => x.Articles)
                .ThenInclude(x => x.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeMark == null)
            {
                return NotFound();
            }

            return View(tradeMark);
        }

        // GET: TradeMarks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TradeMarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] TradeMark tradeMark)
        {
            //VALOR DUPLICADO
            if (_dataContext.TradeMarks.Any(x => x.Name == tradeMark.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{tradeMark.Name}' ya se esta usando";
            }
            else
            if (ModelState.IsValid)
            {
                tradeMark.Name = tradeMark.Name?.ToUpper().Trim();
                tradeMark.Status = true;
                tradeMark.Observation = tradeMark.Observation?.ToUpper().Trim();
                tradeMark.CreationDate = DateTime.Now;
                tradeMark.ModificationDate = DateTime.Now;
                tradeMark.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                tradeMark.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    _dataContext.Add(tradeMark);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Tipo de Artículo Agregado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(tradeMark);
        }

        // GET: TradeMarks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tradeMark = await _dataContext.TradeMarks.FindAsync(id);
            if (tradeMark == null)
            {
                return NotFound();
            }
            return View(tradeMark);
        }

        // POST: TradeMarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] TradeMark tradeMark)
        {
            if (id != tradeMark.Id)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    tradeMark.Name = tradeMark.Name?.ToUpper().Trim();
                    tradeMark.Observation = tradeMark.Observation?.ToUpper().Trim();
                    tradeMark.Status = tradeMark.Status;
                    tradeMark.CreationDate = tradeMark.CreationDate;
                    tradeMark.Creator = tradeMark.Creator;
                    tradeMark.ModificationDate = DateTime.Now;
                    tradeMark.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dataContext.Update(tradeMark);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Tipo de Artículo Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeMarkExists(tradeMark.Id))
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
            return RedirectToAction(nameof(Index));
        }

        // GET: KindofArticle/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var trademark = await _dataContext.TradeMarks
                .FirstOrDefaultAsync(u => u.Id == id);
            if (trademark == null)
            {
                return NotFound();
            }
            return View(trademark);
        }

        // POST: TradeMarks/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var tradeMark = await _dataContext.TradeMarks
                .FirstOrDefaultAsync(u => u.Id == id);
            if (tradeMark != null)
            {
                tradeMark.Status = false;
                tradeMark.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                tradeMark.ModificationDate = DateTime.Now;
            }

            try
            {
                _dataContext.Update(tradeMark);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageLock"] = "Marca de Artículo Bloqueado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: TradeMarks/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tradeMark = await _dataContext.TradeMarks
                .FirstOrDefaultAsync(u => u.Id == id);
            if (tradeMark == null)
            {
                return NotFound();
            }
            return View(tradeMark);
        }

        // POST: TradeMarks/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var trademark = await _dataContext.TradeMarks
                .FirstOrDefaultAsync(u => u.Id == id);
            if (trademark != null)
            {
                trademark.Status = true;
                trademark.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                trademark.ModificationDate = DateTime.Now;
            }
            try
            {
                _dataContext.Update(trademark);
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageUnLock"] = "Marca de Artículo Activado Exitosamente";
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

            var tradeMark = await _dataContext.TradeMarks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tradeMark == null)
            {
                return NotFound();
            }

            return View(tradeMark);
        }

        // POST: TradeMarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tradeMark = await _dataContext.TradeMarks.FindAsync(id);
            if (tradeMark != null)
            {
                _dataContext.TradeMarks.Remove(tradeMark);
            }
            try
            {
                await _dataContext.SaveChangesAsync();
                TempData["AlertMessageDelete"] = "Marca de Artículo Eliminado Exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewBag.ErrorDelete = "Se ha producido un error al borrar el registro, lo más común es que el registro no se puede eliminar debido que se relaciona con alguna notificación o usuario de la base de datos.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TradeMarkExists(int id)
        {
            return _dataContext.TradeMarks.Any(e => e.Id == id);
        }
    }
}