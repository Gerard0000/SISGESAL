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
    public class ArticlesController(DataContext context, ICombosHelper combosHelper) : Controller
    {
        private readonly DataContext _dataContext = context;
        private readonly ICombosHelper _combosHelper = combosHelper;

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Articles.Count();
            ViewBag.Indexcount2 = _dataContext.Articles.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.Articles.Where(m => m.Status == false).Count();

            return View(await _dataContext.Articles
                .Include(m => m.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _dataContext.Articles
                .Include(m => m.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            var model = new ArticleViewModel
            {
                KindofArticles = _combosHelper.GetComboKindofArticles(),
                Suppliers = _combosHelper.GetComboSuppliers(),
                Trademarks = _combosHelper.GetComboTradeMarks()
            };

            return View(model);
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = await ToArticleAsync(model, true);

                try
                {
                    _dataContext.Add(article);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageCreate"] = "Artículo Agregado Exitosamente";
                    return RedirectToAction("Index", "Articles");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.ToString());
                }
            }

            return View(model);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _dataContext.Articles
                .Include(k => k.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(ToArticleViewModel(article));
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //UTILIZAMOS METODO PRIVADO QUE ESTA ABAJO PARA LA EDICIÓN DEL ARTÍCULO
                var article = await ToArticle2Async(model, false);
                try
                {
                    _dataContext.Update(article);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageEdit"] = "Artículo Editado Exitosamente";
                    return RedirectToAction("Index", "Articles");
                }
                catch (Exception)
                {
                    ViewBag.error = "Un error inesperado ha ocurrido";
                }
            }
            return View(model);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _dataContext.Articles
                .Include(k => k.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _dataContext.Articles.FindAsync(id);
            if (article != null)
            {
                _dataContext.Articles.Remove(article);
            }

            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Article/Lock
        public async Task<IActionResult> Lock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = await _dataContext.Articles
                .Include(k => k.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Article/Lock/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockConfirmed(int id)
        {
            var article = await _dataContext.Articles
                 .Include(k => k.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (article != null)
            {
                try
                {
                    article.Status = false;
                    article.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    article.ModificationDate = DateTime.Now;

                    _dataContext.Update(article);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageLock"] = "Artículo Bloqueado Exitosamente";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Article/UnLock
        public async Task<IActionResult> UnLock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = await _dataContext.Articles
                .Include(k => k.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Article/UnLock/5
        [HttpPost, ActionName("UnLock")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockConfirmed(int id)
        {
            var article = await _dataContext.Articles
                .Include(k => k.KindofArticle)
                .Include(m => m.Supplier)
                .Include(m => m.TradeMark)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (article != null)
            {
                try
                {
                    article.Status = true;
                    article.Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    article.ModificationDate = DateTime.Now;

                    _dataContext.Update(article);
                    await _dataContext.SaveChangesAsync();
                    TempData["AlertMessageUnLock"] = "Artículo Activado Exitosamente";
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _dataContext.Articles.Any(e => e.Id == id);
        }

        //OTROS MÉTODOS PRIVADOS
        private async Task<object> ToArticleAsync(ArticleViewModel model, bool isNew)
        {
            var article = new Article

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                Status = true,
                Observation = model.Observation?.Trim().ToUpper(),

                TradeMark = await _dataContext.TradeMarks.FindAsync(model.TrademarkId),
                Supplier = await _dataContext.Suppliers.FindAsync(model.SupplierId),
                KindofArticle = await _dataContext.KindofArticles.FindAsync(model.KindofArticleId),

                CreationDate = DateTime.Now,
                ModificationDate = DateTime.Now,
                Creator = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return article;
        }

        private async Task<object> ToArticle2Async(ArticleViewModel model, bool isNew)
        {
            var article = new Article

            {
                Id = isNew ? 0 : model.Id,

                Name = model.Name?.Trim().ToUpper(),
                Status = model.Status,
                Observation = model.Observation?.Trim().ToUpper(),

                TradeMark = await _dataContext.TradeMarks.FindAsync(model.TrademarkId),
                Supplier = await _dataContext.Suppliers.FindAsync(model.SupplierId),
                KindofArticle = await _dataContext.KindofArticles.FindAsync(model.KindofArticleId),

                CreationDate = model.CreationDate,
                Creator = model.Creator,
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };

            return article;
        }

        private ArticleViewModel ToArticleViewModel(Article article)
        {
            return new ArticleViewModel
            {
                Name = article.Name?.Trim().ToUpper(),
                Status = article.Status,
                Observation = article.Observation?.Trim().ToUpper(),

                KindofArticle = article.KindofArticle,
                TradeMark = article.TradeMark,
                Supplier = article.Supplier,

                CreationDate = article.CreationDate,
                Creator = article.Creator,
                ModificationDate = DateTime.Now,
                Modifier = User.FindFirstValue(ClaimTypes.NameIdentifier),

                Id = article.Id,

                KindofArticleId = article.KindofArticle!.Id,
                TrademarkId = article.TradeMark!.Id,
                SupplierId = article.Supplier!.Id,

                KindofArticles = _combosHelper.GetComboKindofArticles(),
                Trademarks = _combosHelper.GetComboTradeMarks(),
                Suppliers = _combosHelper.GetComboSuppliers(),
            };
        }
    }
}