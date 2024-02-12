using FinallyProjectDataAccess;
using FinallyProjectEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinallyProjectUI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var values = _context.Categories.ToList();
            return View(values);
        }
        [Authorize]
        public IActionResult Upsert(int? id)
        {
            if(id == 0)
            {
                Category category = new Category();
                return View(category);
            }
            else
            {
                var value = _context.Categories.FirstOrDefault(u => u.Id == id);
                return View(value);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(int? id, Category category)
        {
            if (id == null)
            {
                var foundItem = await _context.Categories.FirstOrDefaultAsync(u => u.Name == category.Name);
                if(foundItem != null)
                {
                    TempData["AlertMessage"] = category.Name + " kategorisi listeye eklenemedi aynı isimde kategori mevcut!";
                    return RedirectToAction("Index");
                }
                TempData["AlertMessage"] = category.Name + " kategorisi eklendi!";
                await _context.Categories.AddAsync(category);
            }
            else
            {
                var items = await _context.Categories.FirstOrDefaultAsync(u => u.Id == id);
                items.Name = category.Name;
                TempData["AlertMessage"] = category.Name + " kategorisi güncellendi!";
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var value = _context.Categories.FirstOrDefault(u => u.Id == id);
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            var value = _context.Categories.FirstOrDefault(u => u.Id == category.Id);
            _context.Categories.Remove(value);
            TempData["AlertMessage"] = category.Name + " kategorisi silindi!!";
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
