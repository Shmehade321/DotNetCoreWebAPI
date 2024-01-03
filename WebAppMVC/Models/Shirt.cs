using System.ComponentModel.DataAnnotations;
using WebAppMVC.Models.Validations;

namespace WebAppMVC.Models
{
    public class Shirt
    {
        public int Id { get; set; }

        [Required]
        public string? Brand { get; set; }

        [Required]
        public string? Color { get; set; }

        [EnsureCorrectSize]
        public int? Size { get; set; }

        [Required]
        public string? Gender { get; set; }

        public double? Price { get; set; }
    }
}
