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
            //USUARIOS ACTIVOS
            ViewBag.Indexcount = _dataContext.Customers.Count();
            //USUARIOS BLOQUEADOS
            ViewBag.Indexcount2 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.LockoutEnd == null).Count();
            //TOTAL DE USUARIOS
            ViewBag.Indexcount3 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.LockoutEnd > DateTime.Now).Count();
            //USUARIOS ACTIVOS CON ALMACÉN
            ViewBag.Indexcount4 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User.LockoutEnd == null)).Count();
            //USUARIOS BLOQUEADOS CON ALMACÉN
            ViewBag.Indexcount5 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot != null) && (m.User!.LockoutEnd != null)).Count();
            //TOTAL DE USUARIOS CON ALMACÉN
            ViewBag.Indexcount6 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.Depot != null).Count();
            //USUARIOS ACTIVOS SIN ALMACÉN
            ViewBag.Indexcount7 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.User.LockoutEnd == null)).Count();
            //USUARIOS BLOQUEADOS SIN ALMACÉN
            ViewBag.Indexcount8 = _dataContext.Customers.Include(x => x.User).Where(m => (m.User!.Depot == null) && (m.User!.LockoutEnd != null)).Count();
            //TOTAL DE USUARIOS SIN ALMACÉN
            ViewBag.Indexcount9 = _dataContext.Customers.Include(x => x.User).Where(m => m.User!.Depot == null).Count();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
