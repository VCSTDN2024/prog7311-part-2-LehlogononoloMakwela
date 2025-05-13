using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AgriEnergyConnect_ST10290044_Part2.Models
{
    public class Product
    {
        
        
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string Category { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime ProductionDate { get; set; }

            public string ImageUrl { get; set; }

        // Foreign key to link to Farmer (User)
        public string FarmerId { get; set; }

            // Reference to your custom Users class
            [ForeignKey("FarmerId")]
            public Users Farmer { get; set; }
        }
}
