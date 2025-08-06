using ChatNest.Models;
using ChatNest.Models.Data;
using ChatNest.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatNest.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(vmLogin model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Invalid Credentials";
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("UserId", user.Id.ToString())
    };

            var identity = new ClaimsIdentity(claims, "MohsinMadeCookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MohsinMadeCookies", principal);
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Login successfull";
            return RedirectToAction("Index", "Chat");
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(vmLogin model)
        {
            if (!ModelState.IsValid) return View(model);
            var userExists = await _context.Users.AnyAsync(u => u.Username == model.Username);
            if (userExists)
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Username already taken";
                return View(model);
            }
            var user = new User
            {
                Username = model.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedAt = DateTime.Now,
                ConnectionId = ""
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Registeration successfull";
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Logout successfull";
            return RedirectToAction("Login");
        }
    }
}
