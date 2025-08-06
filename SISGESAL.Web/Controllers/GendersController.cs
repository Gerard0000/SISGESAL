using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;

namespace SISGESAL.web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class GendersController(DataContext context) : Controller
    {
        private readonly DataContext _context = context;

        // GET: Genders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Genders
                .Include(d => d.KindofPeople)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }
    }
}