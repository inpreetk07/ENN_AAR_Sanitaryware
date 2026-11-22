
using ENN_AAR_Sanitaryware.Data;
using ENN_AAR_Sanitaryware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ENN_AAR_Sanitaryware.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.IsAvailable)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEnquiry(Enquiry enquiry)
        {
            if (!ModelState.IsValid)
            {
                TempData["EnquiryError"] = "Please enter valid enquiry details.";
                return RedirectToAction(nameof(Index));
            }

            enquiry.Status = "Pending";
            enquiry.CreatedAt = DateTime.UtcNow;

            _context.Enquiries.Add(enquiry);
            await _context.SaveChangesAsync();

            TempData["EnquirySuccess"] = "Your enquiry has been submitted successfully!";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
