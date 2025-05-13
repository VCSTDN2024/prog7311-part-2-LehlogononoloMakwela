using AgriEnergyConnect_ST10290044_Part2.Data;
using AgriEnergyConnect_ST10290044_Part2.Models;
using AgriEnergyConnect_ST10290044_Part2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect_ST10290044_Part2.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public EmployeesController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // List all the farmers
        public async Task<IActionResult> Farmers()
        {
            // Load farmers with linked user info
            var farmers = await _context.FarmerProfiles
                .Include(f => f.User) 
                .ToListAsync();

            return View(farmers);
        }

        // Show form to add farmer
        public async Task<IActionResult> AddFarmer()
        {
            
            ViewBag.Users = await _context.Users.ToListAsync();
            var model = new FarmerProfile { UserId = "" };
            return View(model);
           
        }

        // Handle form submission to add farmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFarmer(FarmerProfile model)
        {
            ModelState.Remove("User"); // Removes the User property from validation to prevent errors 

            if (ModelState.IsValid)
            {
                try
                {
                    model.DateAdded = DateTime.Now;
                    _context.FarmerProfiles.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Farmers));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            ViewBag.Users = await _context.Users.ToListAsync();
            return View(model);
        }





        // List and filter products
        public async Task<IActionResult> AllProducts([FromQuery] ProductFilterViewModel model)
        {
            var query = _context.Products.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(model.FarmerId))
                query = query.Where(p => p.FarmerId == model.FarmerId);

            if (model.StartDate.HasValue)
                query = query.Where(p => p.ProductionDate >= model.StartDate.Value);

            if (model.EndDate.HasValue)
                query = query.Where(p => p.ProductionDate <= model.EndDate.Value);

            if (!string.IsNullOrEmpty(model.ProductType))
                query = query.Where(p => p.Category == model.ProductType);

            // Include farmer info
            var products = await query.Include(p => p.Farmer).ToListAsync();

           
            var farmers = await _context.FarmerProfiles.Include(f => f.User).ToListAsync();

            
            model.Farmers = farmers;
            model.Products = products;

            return View(model);
        }

        public async Task<IActionResult> Dashboard()
        {
            // Count total farmers
            int totalFarmers = await _context.FarmerProfiles.CountAsync();

            // Get the latest 5 farmers 
            var recentFarmers = await _context.FarmerProfiles
                .Include(f => f.User)
                .OrderByDescending(f => f.DateAdded) 
                .Take(5)
                .ToListAsync();

            // Count total products
            int totalProducts = await _context.Products.CountAsync();

            // Count recent products (last 5)
            int recentProductsCount = await _context.Products
                .OrderByDescending(p => p.ProductionDate)
                .Take(5)
                .CountAsync();

            // Pass data via ViewBag
            ViewBag.TotalFarmers = totalFarmers;
            ViewBag.RecentFarmers = recentFarmers;
            ViewBag.TotalProducts = totalProducts;
            ViewBag.RecentProductsCount = recentProductsCount;

            return View();
        }

        // Delete farmer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFarmer(int id)
        {
            var farmer = await _context.FarmerProfiles.FindAsync(id);
            if (farmer != null)
            {
                _context.FarmerProfiles.Remove(farmer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Farmers));
        }
    }
}
