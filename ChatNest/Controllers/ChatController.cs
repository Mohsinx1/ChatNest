using ChatNest.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatNest.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;
        private int LoggedInUserId => int.Parse(User.FindFirstValue("UserId"));

        public ChatController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Where(u => u.Id != LoggedInUserId)
                .ToListAsync();

            ViewBag.Contacts = users;

            return View();
        }
    }
}
