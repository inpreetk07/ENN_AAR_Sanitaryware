
using ENN_AAR_Sanitaryware.Data;
using ENN_AAR_Sanitaryware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ENN_AAR_Sanitaryware.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category updatedCategory)
        {
            if (id != updatedCategory.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(updatedCategory);

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            bool exists = await _context.Categories
                .AnyAsync(c => c.Id != id &&
                               c.Name.ToLower() == updatedCategory.Name.ToLower());

            if (exists)
            {
                ModelState.AddModelError("Name", "This category already exists.");
                return View(updatedCategory);
            }

            var oldName = category.Name;

            category.Name = updatedCategory.Name;
            category.Description = updatedCategory.Description;

            var products = await _context.Products
                .Where(p => p.Category == oldName)
                .ToListAsync();

            foreach (var product in products)
            {
                product.Category = updatedCategory.Name;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            bool exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());

            if (exists)
            {
                ModelState.AddModelError("Name", "This category already exists.");
                return View(category);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            bool inUse = await _context.Products
                .AnyAsync(p => p.Category == category.Name);

            if (inUse)
            {
                TempData["Error"] = "This category is used by products and cannot be deleted.";
                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
