using System;
using System.Collections.Generic;
using System.Linq;
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
    public class KindofPeoplesController : Controller
    {
        private readonly DataContext _dataContext;

        public KindofPeoplesController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: KindofPeoples
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.KindofPeoples.Count();
            return View(await _dataContext.KindofPeoples
                .Include(x => x.Genders)
                .ToListAsync());
        }

        // GET: KindofPeoples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kindofPeople = await _dataContext.KindofPeoples
                .Include(x => x.Genders!)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindofPeople == null)
            {
                return NotFound();
            }

            return View(kindofPeople);
        }

        private bool KindofPeopleExists(int id)
        {
            return _dataContext.KindofPeoples.Any(e => e.Id == id);
        }
    }
}
