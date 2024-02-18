using FinallyProjectDataAccess;
using FinallyProjectEntity.ViewModel;
using FinallyProjectUI.Models;
using FinallyProjectUI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FinallyProjectUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index(string? searchByName, string? searchByCategory)
        {
            var claim = _signInManager.IsSignedIn(User);
            if (claim)
            {
                var userId = _userManager.GetUserId(User);
                var count = _db.userCarts.Where(u => u.userId.Contains(userId)).Count();
                HttpContext.Session.SetInt32(cartCount.sessionCount, count);
            }


            HomePageVM vm = new HomePageVM();
            if(searchByName != null)
            {
                vm.ProductList = _db.Products.Where(productName => EF.Functions.Like(productName.Name, $"%{searchByName}")).ToList();
                vm.categories = _db.Categories.ToList();
            }
            else if (searchByCategory != null)
            {
                var searchByCategoryName = _db.Categories.FirstOrDefault(u => u.Name == searchByCategory);
                vm.ProductList = _db.Products.Where(u => u.CategoryId == searchByCategoryName.Id).ToList();
                vm.categories = _db.Categories.Where(u => u.Name.Contains(searchByCategory));
            }
            else
            {
                vm.ProductList = _db.Products.ToList();
                vm.categories = _db.Categories.ToList();
            }
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
