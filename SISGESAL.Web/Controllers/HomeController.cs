using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SISGESAL.web.Data;
using SISGESAL.web.Models;

namespace SISGESAL.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, DataContext context) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly DataContext _dataContext = context;

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
