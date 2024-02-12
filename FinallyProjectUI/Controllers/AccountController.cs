using FinallyProjectEntity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinallyProjectUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginVM vm = new LoginVM();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent:false, lockoutOnFailure:true);
            if (result.Succeeded)
            {
                RedirectToAction("Index", "Home");
            }
            else
            {
                login.LoginStatus = "unsuccessfull";
            }
            
            return View(login);
        }
    }
}
