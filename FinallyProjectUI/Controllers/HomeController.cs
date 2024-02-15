using FinallyProjectDataAccess;
using FinallyProjectEntity.ViewModel;
using FinallyProjectUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FinallyProjectUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomePageVM vm = new HomePageVM();
            vm.ProductList =_db.Products.ToList();
            vm.categories = _db.Categories.ToList();
            return View(vm);
        }

        public IActionResult Privacy()
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
