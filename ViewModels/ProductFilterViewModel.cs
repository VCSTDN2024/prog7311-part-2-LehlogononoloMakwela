using AgriEnergyConnect_ST10290044_Part2.Models;

namespace AgriEnergyConnect_ST10290044_Part2.ViewModels
{
    public class ProductFilterViewModel
    {
        public string FarmerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProductType { get; set; }

        public List<FarmerProfile> Farmers { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
