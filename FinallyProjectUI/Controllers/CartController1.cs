using FinallyProjectDataAccess;
using FinallyProjectEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.AccessControl;

namespace FinallyProjectUI.Controllers
{
    public class CartController1 : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CartController1(ApplicationDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var productAddToCart = await _db.Products.FirstOrDefaultAsync(u => u.Id == productId);
            var CheckIfUserSignedInOrNot = _signInManager.IsSignedIn(User);
            if (CheckIfUserSignedInOrNot)
            {
                var user = _userManager.GetUserId(User);
                if(user!= null)
                {
                    var getTheCartIfAnyExistForTheUser = await _db.userCarts.Where(u => u.userId.Contains(user)).ToListAsync();
                    if(getTheCartIfAnyExistForTheUser.Count() > 0)
                    {
                        var getTheQuantity = getTheCartIfAnyExistForTheUser.FirstOrDefault(p => p.ProductId == productId);
                        if(getTheQuantity!=null)
                        {
                            getTheQuantity.Quantity = getTheQuantity.Quantity + 1;
                            _db.userCarts.Update(getTheQuantity);
                        }
                        else
                        {
                            userCart newItemToCart = new userCart
                            {
                                productId = productId,
                                userId = user,
                                Quantity = 1,
                            };
                        }
                    }
                }
            }
            return View();
        }
    }
}
