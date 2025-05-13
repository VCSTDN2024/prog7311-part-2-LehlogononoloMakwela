using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnect_ST10290044_Part2.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
