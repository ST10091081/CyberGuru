using Microsoft.AspNetCore.Mvc.RazorPages;
using CyberGamify.Data;
using CyberGamify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CyberGamify.Pages
{
    public class LeaderboardModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public LeaderboardModel(ApplicationDbContext db) => _db = db;

        public List<ApplicationUser> TopUsers { get; set; }

        public void OnGet()
        {
            // Top 10 users by XP
            TopUsers = _db.Users
                          .OrderByDescending(u => u.XP)
                          .Take(10)
                          .ToList();
        }
    }
}
