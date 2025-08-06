using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class KindofArticlesController(DataContext context) : Controller
    {
        private readonly DataContext _dataContext = context;

        // GET: KindofArticles
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.KindofArticles.Count();
            ViewBag.Indexcount2 = _dataContext.KindofArticles.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.KindofArticles.Where(m => m.Status == false).Count();
            return View(await _dataContext.KindofArticles
                .Include(x => x.Articles)
                .ToListAsync());
        }

        // GET: KindofArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindofArticle = await _dataContext.KindofArticles
                .Include(x => x.Articles!)
                .ThenInclude(x => x.TradeMark)
                .Include(x => x.Articles!)
                .ThenInclude(x => x.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindofArticle == null)
            {
                return NotFound();
            }

            return View(kindofArticle);
        }

        // GET: KindofArticles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KindofArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] KindofArticle kindofArticle)
        {
            //VALOR DUPLICADO
            if (_dataContext.KindofArticles.Any(x => x.Name == kindofArticle.Name))
            {
                ViewBag.DuplicateMessage = $"El nombre '{kindofArticle.Name}' ya se esta usando";
            }
            else
            if (ModelState.IsValid)
            {
                kindofArticle.Name = kindofArticle.Name?.ToUpper().Trim();
                kindofArticle.Status = true;
                kindofArticle.Observation = kindofArticle.Observation?.ToUpper().Trim();
                kindofArticle.CreationDate = DateTime.Now;
                kindofArticle.ModificationDate = DateTime.Now;
                kindofArticle.Creator = User.FindFirstValue(ClaimTypes.NameIdentifier);
                kindofArticle.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    _dataContext.Add(kindofArticle);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Tipo de Artículo Agregado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ViewBag.DuplicateMessage = "Se ha producido un error ó el valor esta duplicado con otro valor de la base de datos";
                }
            }
            return View(kindofArticle);
        }

        // GET: KindofArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindofArticle = await _dataContext.KindofArticles.FindAsync(id);
            if (kindofArticle == null)
            {
                return NotFound();
            }
            return View(kindofArticle);
        }

        // POST: KindofArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] KindofArticle kindofArticle)
        {
            if (id != kindofArticle.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    kindofArticle.Name = kindofArticle.Name?.ToUpper().Trim();
                    kindofArticle.Observation = kindofArticle.Observation?.ToUpper().Trim();
                    kindofArticle.Status = kindofArticle.Status;
                    kindofArticle.CreationDate = kindofArticle.CreationDate;
                    kindofArticle.Creator = kindofArticle.Creator;
                    kindofArticle.ModificationDate = DateTime.Now;
                    kindofArticle.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _dataContext.Update(kindofArticle);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Tipo de Artículo Editado Exitosamente";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KindofArticleExists(kindofArticle.Id))
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
            return View(kindofArticle);
        }

        // GET: KindofArticle/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kindofArticle = await _dataContext.KindofArticles
                .FirstOrDefaultAsync(u => u.Id == id);
            if (kindofArticle == null)
            {
                return NotFound();
            }
            return View(kindofArticle);
        }

        // POST: KindofArticle/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var kindofArticle = await _dataContext.KindofArticles
                .FirstOrDefaultAsync(u => u.Id == id);
            if (kindofArticle != null)
            {
                try
                {
                    kindofArticle.Status = false;
                    kindofArticle.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    kindofArticle.ModificationDate = DateTime.Now;

                    _dataContext.Update(kindofArticle);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageLock"] = "Tipo de Artículo Bloqueado Exitosamente";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: KindofArticle/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kindofArticle = await _dataContext.KindofArticles
                .FirstOrDefaultAsync(u => u.Id == id);
            if (kindofArticle == null)
            {
                return NotFound();
            }
            return View(kindofArticle);
        }

        // POST: KindofArticle/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var kindofArticle = await _dataContext.KindofArticles
                .FirstOrDefaultAsync(u => u.Id == id);
            if (kindofArticle != null)
            {
                try
                {
                    kindofArticle.Status = true;
                    kindofArticle.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    kindofArticle.ModificationDate = DateTime.Now;

                    _dataContext.Update(kindofArticle);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageUnLock"] = "Tipo de Artículo Activado Exitosamente";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: TradeMarks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindofArticle = await _dataContext.KindofArticles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindofArticle == null)
            {
                return NotFound();
            }

            return View(kindofArticle);
        }

        // POST: KindofArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kindofArticle = await _dataContext.KindofArticles.FindAsync(id);
            if (kindofArticle != null)
            {
                _dataContext.KindofArticles.Remove(kindofArticle);
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

        private bool KindofArticleExists(int id)
        {
            return _dataContext.KindofArticles.Any(e => e.Id == id);
        }
    }
}