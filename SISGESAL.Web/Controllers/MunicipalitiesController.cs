using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Models;
using System.Security.Claims;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class MunicipalitiesController : Controller
    {
        private readonly DataContext _dataContext;

        public MunicipalitiesController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: Municipalities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var municipality = await _dataContext.Municipalities
                .Include(x => x.Courts!)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (municipality == null)
            {
                return NotFound();
            }

            return View(municipality);
        }
    }
}