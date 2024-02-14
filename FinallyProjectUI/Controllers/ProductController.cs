using FinallyProjectDataAccess;
using FinallyProjectEntity;
using FinallyProjectEntity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinallyProjectUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _HostEnvironment;

        public ProductController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _HostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.category).ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productsVM = new ProductVM()
            {
                Inventories = new Inventory(),
                PImages = new PImages(),
                CategoriesList = _context.Categories.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(productsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            string homeImageUrl = "";
            if(productVM.Images != null)
            {
                foreach(var image in productVM.Images)
                {
                    homeImageUrl = image.FileName;
                    if (homeImageUrl.Contains("Home"))
                    {
                        homeImageUrl = UploadFiles(image);
                        break;
                    }
                }
            }
            productVM.Products.HomeImageUrl = homeImageUrl;
            await _context.AddAsync(productVM.Products);
            await _context.SaveChangesAsync();
            var NewProduct = await _context.Products.Include(u => u.category).FirstOrDefaultAsync(u => u.Name == productVM.Products.Name);
            productVM.Inventories.Name = NewProduct.Name;
            productVM.Inventories.Category = NewProduct.category.Name;
            await _context.Inventories.AddAsync(productVM.Inventories);
            await _context.SaveChangesAsync();

            if(productVM.Images != null)
            {
                foreach(var image in productVM.Images)
                {
                    string tempFileName = image.FileName;
                    if (!tempFileName.Contains("Home"))
                    {
                        string stringFileName = UploadFiles(image);
                        var addressImage = new PImages
                        {
                            ImageUrl = stringFileName,
                            ProductId = NewProduct.Id,
                            ProductName = NewProduct.Name
                        };
                        await _context.PImages.AddAsync(addressImage);
                    }
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Product");
        }

        private string UploadFiles(IFormFile image)
        {
            string fileName = null;
            if(image != null)
            {
                string uploadDirLocation = Path.Combine(_HostEnvironment.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadDirLocation, fileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}
