using AgriEnergyConnect_ST10290044_Part2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect_ST10290044_Part2.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        
        public DbSet<Product> Products { get; set; }

        public DbSet<FarmerProfile> FarmerProfiles { get; set; }
    }
}
