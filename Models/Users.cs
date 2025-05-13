using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect_ST10290044_Part2.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }

}
