using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyConnect_ST10290044_Part2.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmersController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
