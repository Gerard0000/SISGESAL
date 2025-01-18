using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;

namespace SISGESAL.web.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly DataContext _dataContext;

        public ArticlesController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Articles.Count();
            ViewBag.Indexcount2 = _dataContext.Articles.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.Articles.Where(m => m.Status == false).Count();
            return View(await _dataContext.Article
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

            var article = await _dataContext.Article
                .Include(m => m.KindofArticle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kindofArticle = await _dataContext.KindofArticles.FindAsync(id.Value);
            if (kindofArticle == null)
            {
                return NotFound();
            }

            var model = new ArticleViewModel
            {
                KindofArticleId = kindofArticle.Id
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
                    return RedirectToAction("Details", "KindofArticles", new { @id = model.KindofArticleId });
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

            var article = await _dataContext.Article
                .Include(k => k.KindofArticle)
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
                    return RedirectToAction("Details", "KindofArticles", new { @id = model.KindofArticleId });
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

            var article = await _dataContext.Article
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
            var article = await _dataContext.Article.FindAsync(id);
            if (article != null)
            {
                _dataContext.Article.Remove(article);
            }

            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _dataContext.Article.Any(e => e.Id == id);
        }

        //OTROS MÉTODOS PRIVADOS
        private async Task<object> ToArticleAsync(ArticleViewModel model, bool v)
        {
            throw new NotImplementedException();
        }

        private string? ToArticleViewModel(Article article)
        {
            throw new NotImplementedException();
        }

        private async Task<object> ToArticle2Async(ArticleViewModel model, bool v)
        {
            throw new NotImplementedException();
        }
    }
}
