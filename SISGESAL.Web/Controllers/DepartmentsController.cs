using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class DepartmentsController : Controller
    {
        private readonly DataContext _dataContext;

        public DepartmentsController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            ViewBag.Indexcount = _dataContext.Departments.Count();
            return View(await _dataContext.Departments
                .Include(x => x.Municipalities)
                .ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _dataContext.Departments
                .Include(x => x.Municipalities!)
                .ThenInclude(c => c.Courts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        //*******************************************INTENTAR DROPDOWNLIST EN CASCADA****************************
        //[HttpGet("combo")]
        //public async Task<ActionResult> GetCombo()
        //{
        //    return Ok(await _dataContext.Departments.ToListAsync());
        //}
        //********************************************************************************************************

        private bool DepartmentExists(int id)
        {
            return _dataContext.Departments.Any(e => e.Id == id);
        }
    }
}