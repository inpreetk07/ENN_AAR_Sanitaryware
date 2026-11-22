
using ENN_AAR_Sanitaryware.Data;
using ENN_AAR_Sanitaryware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ENN_AAR_Sanitaryware.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnquiriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnquiriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var enquiries = await _context.Enquiries
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return View(enquiries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var enquiry = await _context.Enquiries.FindAsync(id);

            if (enquiry == null)
                return NotFound();

            if (status != "Pending" &&
                status != "Contacted" &&
                status != "Completed")
            {
                return BadRequest();
            }

            enquiry.Status = status;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var enquiry = await _context.Enquiries.FindAsync(id);

            if (enquiry == null)
                return NotFound();

            _context.Enquiries.Remove(enquiry);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
