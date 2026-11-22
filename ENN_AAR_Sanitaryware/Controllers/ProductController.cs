
using ENN_AAR_Sanitaryware.Data;
using ENN_AAR_Sanitaryware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ENN_AAR_Sanitaryware.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? image)
        {
            if (image != null && image.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension) || image.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImagePath",
                        "Upload JPG, JPEG, PNG or WEBP image up to 5 MB.");
                }
                else
                {
                    var uploadsFolder = Path.Combine(
                        _environment.WebRootPath, "uploads", "products");

                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);

                    product.ImagePath = "/uploads/products/" + fileName;
                }
            }

            if (!ModelState.IsValid)
                return View(product);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();
            ViewBag.Categories = await _context.Categories
    .OrderBy(c => c.Name)
    .ToListAsync();
            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Product updatedProduct,
            IFormFile? image)
        {
            if (id != updatedProduct.Id)
                return BadRequest();

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            string? newImagePath = null;

            if (image != null && image.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension) ||
                    image.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        "ImagePath",
                        "Upload JPG, JPEG, PNG or WEBP image up to 5 MB.");
                }
                else
                {
                    var uploadsFolder = Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "products");

                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);

                    newImagePath = "/uploads/products/" + fileName;
                }
            }
            ViewBag.Categories = await _context.Categories
    .OrderBy(c => c.Name)
    .ToListAsync();
            if (!ModelState.IsValid)
            {
                if (newImagePath != null)
                {
                    var savedPath = Path.Combine(
                        _environment.WebRootPath,
                        newImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(savedPath))
                        System.IO.File.Delete(savedPath);
                }

                return View(updatedProduct);
            }

            var oldImagePath = product.ImagePath;

            product.Name = updatedProduct.Name;
            product.Category = updatedProduct.Category;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.IsAvailable = updatedProduct.IsAvailable;

            if (newImagePath != null)
                product.ImagePath = newImagePath;

            await _context.SaveChangesAsync();

            // Delete the old uploaded image after the database update succeeds.
            if (newImagePath != null &&
                !string.IsNullOrEmpty(oldImagePath) &&
                oldImagePath.StartsWith("/uploads/products/"))
            {
                var oldFilePath = Path.Combine(
                    _environment.WebRootPath,
                    oldImagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAvailability(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            product.IsAvailable = !product.IsAvailable;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            // Delete uploaded image
            if (!string.IsNullOrEmpty(product.ImagePath) &&
                product.ImagePath.StartsWith("/uploads/products/"))
            {
                var imagePath = Path.Combine(
                    _environment.WebRootPath,
                    product.ImagePath.TrimStart('/').Replace(
                        '/',
                        Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
