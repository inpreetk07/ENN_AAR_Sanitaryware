
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ENN_AAR_Sanitaryware.Data;
using Microsoft.EntityFrameworkCore;

namespace ENN_AAR_Sanitaryware.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _context;

        public AdminController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(
                username, password, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = result.IsLockedOut
                ? "Too many failed attempts. Please try again in 15 minutes."
                : "Invalid username or password.";
            return View();
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalProducts = await _context.Products.CountAsync();
            ViewBag.TotalEnquiries = await _context.Enquiries.CountAsync();

            ViewBag.AvailableProducts = await _context.Products
                .CountAsync(p => p.IsAvailable);
            ViewBag.UnavailableProducts = await _context.Products
    .CountAsync(p => !p.IsAvailable);

            ViewBag.TotalCategories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .CountAsync();
            ViewBag.RecentProducts = await _context.Products
    .OrderByDescending(p => p.CreatedAt)
    .Take(5)
    .ToListAsync();

            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(
            string currentPassword,
            string newPassword,
            string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "New password and confirm password do not match.";
                return View();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _userManager.ChangePasswordAsync(
                user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);

                ViewBag.Success = "Password changed successfully!";
                return View();
            }

            ViewBag.Error = string.Join(" ",
                result.Errors.Select(error => error.Description));

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
