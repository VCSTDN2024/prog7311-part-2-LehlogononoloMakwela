using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriEnergyConnect_ST10290044_Part2.Models
{
    //to store farmers info includes validation
    public class FarmerProfile
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Farmer Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        
        public string UserId { get; set; }

       
        public Users User { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
