using AgriEnergyConnect_ST10290044_Part2.Data;
using AgriEnergyConnect_ST10290044_Part2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect_ST10290044_Part2.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public ProductsController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //  Show form to add new product
        public IActionResult Add()
        {
            return View();
        }

        //  Handle new product creation with image upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imagePath = null;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    var extension = Path.GetExtension(model.ImageFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    Directory.CreateDirectory(uploadsFolder); 

                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    // Save relative URL for the image
                    imagePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                }

                var user = await _userManager.GetUserAsync(User);
                var product = new Product
                {
                    Name = model.Name,
                    Category = model.Category,
                    ProductionDate = model.ProductionDate,
                    FarmerId = user.Id,
                    ImageUrl = imagePath
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyProducts));
            }

            return View(model);
        }

        //  List products added by current user
        public async Task<IActionResult> MyProducts()
        {
            var user = await _userManager.GetUserAsync(User);
            var products = await _context.Products
                .Where(p => p.FarmerId == user.Id)
                .ToListAsync();

            return View(products);
        }

        // Show all products for marketplace (public)
        [AllowAnonymous]
        public async Task<IActionResult> Marketplace()
        {
            var products = await _context.Products
                .Include(p => p.Farmer)
                .ToListAsync();
            return View(products);
        }

        // Farmer Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        //  Delete a product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Product deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Product not found.";
            }
            return RedirectToAction(nameof(MyProducts));
        }
    }
}
    

