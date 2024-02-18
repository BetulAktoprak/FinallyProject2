using FinallyProjectDataAccess;
using FinallyProjectEntity;
using FinallyProjectEntity.ViewModel;
using FinallyProjectUI.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinallyProjectUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public OrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult orderDetailPreview()
        {
            var claim = _signInManager.IsSignedIn(User);
            if (claim)
            {
                var userId = _userManager.GetUserId(User);
                var currentUser = _db.applicationUser.FirstOrDefault(x => x.Id == userId);
                SummeryVM summeryVM = new SummeryVM()
                {
                    userCartList = _db.userCarts.Include(u => u.product).Where(u => u.userId.Contains(userId)).ToList(),
                    orderSummery = new UserOrderHeader(),
                    cartUserId = userId,
                };
                if (currentUser != null)
                {
                    summeryVM.orderSummery.DeliveryStreetAddress = currentUser.Address;
                    summeryVM.orderSummery.City = currentUser.City;
                    summeryVM.orderSummery.State = currentUser.State;
                    summeryVM.orderSummery.PostalCode = currentUser.PostalCode;
                    summeryVM.orderSummery.PhoneNumber = currentUser.PhoneNumber;
                    summeryVM.orderSummery.Name = currentUser.FirstName + " " + currentUser.LastName;
                }
                var count = _db.userCarts.Where(u => u.userId.Contains(_userManager.GetUserId(User))).ToList().Count;
                HttpContext.Session.SetInt32(cartCount.sessionCount, count);
                return View(summeryVM);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Summery(SummeryVM summeryVMFromView)
        {
            var claim = _signInManager.IsSignedIn(User);
            if (claim)
            {
                var userId = _userManager.GetUserId(User);
                var currentUser = _db.applicationUser.FirstOrDefault(x => x.Id == userId);
                SummeryVM summeryVM = new SummeryVM()
                {
                    userCartList = _db.userCarts.Include(u => u.product).Where(u => u.userId.Contains(userId)).ToList(),
                    orderSummery = new UserOrderHeader(),
                };
                if (currentUser != null)
                {
                    summeryVM.orderSummery.Name = summeryVMFromView.orderSummery.Name;
                    summeryVM.orderSummery.DeliveryStreetAddress = summeryVMFromView.orderSummery.DeliveryStreetAddress;
                    summeryVM.orderSummery.City = summeryVMFromView.orderSummery.City;
                    summeryVM.orderSummery.State = summeryVMFromView.orderSummery.State;
                    summeryVM.orderSummery.PostalCode = summeryVMFromView.orderSummery.PostalCode;
                    summeryVM.orderSummery.PhoneNumber = summeryVMFromView.orderSummery.PhoneNumber;
                    summeryVM.orderSummery.DateOfOrder = DateTime.Now;
                    summeryVM.orderSummery.OrderStatus = "Pending";
                    summeryVM.orderSummery.PaymentStatus = "Not Paid";
                    await _db.AddAsync(summeryVM.orderSummery);
                    await _db.SaveChangesAsync();

                }
                return RedirectToAction("OrderSuccess", new { id = summeryVM.orderSummery.Id });
                
            }
            return View();
        }

        public IActionResult OrderCancele()
        {
            return RedirectToAction("CardIndex", "Cart");
        }

        public IActionResult OrderSuccess(int id)
        {
            var claim = _signInManager.IsSignedIn(User);
            if (claim)
            {
                var userId = _userManager.GetUserId(User);
                var UserCartRemove = _db.userCarts.Where(u => u.userId.Contains(userId)).ToList();
                var orderProcessed = _db.orderHeaders.FirstOrDefault(h => h.Id == id);
                if (orderProcessed != null)
                {
                    if (orderProcessed.PaymentStatus == "Not Paid")
                    {
                        orderProcessed.PaymentStatus = "Paid";
                        orderProcessed.PaymentProccessDate = DateTime.Now;
                    }
                }
                foreach (var list in UserCartRemove)
                {
                    OrderDetails orderReceived = new OrderDetails()
                    {
                        OrderHeaderId = orderProcessed.Id,
                        ProductId = (int)list.ProductId,
                        Count = list.Quantity,
                    };
                    _db.orderDetails.Add(orderReceived);
                }
                ViewBag.OrderId = id;
                _db.userCarts.RemoveRange(UserCartRemove);
                _db.SaveChanges();
                var count = _db.userCarts.Where(u => u.userId.Contains(_userManager.GetUserId(User))).ToList().Count;
                HttpContext.Session.SetInt32(cartCount.sessionCount, count);

            }
            return View();
        }

    }
}
