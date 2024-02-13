using FinallyProjectEntity;
using FinallyProjectEntity.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinallyProjectUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                login.LoginStatus = "unsuccessfull";
            }
            
            return View(login);
        }

        public IActionResult Register()
        {
            RegisterVM vm = new RegisterVM();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            var user = new ApplicationUser
            {
                FirstName = register.applicationUser.FirstName,
                LastName = register.applicationUser.LastName,
                Email = register.Email,
                UserName = register.UserName,
                Address = register.applicationUser.Address,
                City = register.applicationUser.City
            };
            var Registration = await _userManager.CreateAsync(user, register.Password);
            if (Registration.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                register.StatusMessage = "Kayıt Başarılı!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                register.StatusMessage = "Kayıt Başarısız!";
            }
            return View(register);
        }
    }
}
