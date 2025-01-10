using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;

namespace SISGESAL.web.Controllers
{
    public class DepotsController : Controller
    {
        private readonly DataContext _dataContext;

        public DepotsController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Depots
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Depots.Count();
            ViewBag.Indexcount2 = _dataContext.Depots.Where(m => m.Status == true).Count();
            ViewBag.Indexcount3 = _dataContext.Depots.Where(m => m.Status == false).Count();
            return View(await _dataContext.Depots
                .ToListAsync());
        }

        // GET: Depots/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depot = await _dataContext.Depots
                .Include(d => d.User)
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
            return View();
        }

        // POST: Depots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] Depot depot)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(depot);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(depot);
        }

        // GET: Depots/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depot = await _dataContext.Depots.FindAsync(id);
            if (depot == null)
            {
                return NotFound();
            }
            return View(depot);
        }

        // POST: Depots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,Observation,CreationDate,Creator,ModificationDate,Modifier")] Depot depot)
        {
            if (id != depot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(depot);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepotExists(depot.Id))
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
            return View(depot);
        }

        private bool DepotExists(int id)
        {
            return _dataContext.Depots.Any(e => e.Id == id);
        }
    }
}