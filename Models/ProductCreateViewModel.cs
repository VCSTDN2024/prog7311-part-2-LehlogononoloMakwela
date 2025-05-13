using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect_ST10290044_Part2.Models
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}
