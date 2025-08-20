using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CyberGamify.Models;

namespace CyberGamify.Data
{
    // Use IdentityDbContext<ApplicationUser> so Identity tables + our DbSets work together
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<GameResult> GameResults { get; set; }
    }
}
