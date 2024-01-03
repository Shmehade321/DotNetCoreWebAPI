using System.ComponentModel.DataAnnotations;

namespace WebAppMVC.Models.Validations
{
    public class EnsureCorrectSizeAttribute : ValidationAttribute
    {
        private const string GENDER_MEN = "men";
        private const string GENDER_WOMEN = "women";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirt = validationContext.ObjectInstance as Shirt;
            if (shirt != null && !string.IsNullOrWhiteSpace(shirt.Gender))
            {
                if(shirt.Gender.Equals(GENDER_MEN, StringComparison.OrdinalIgnoreCase) && shirt.Size < 8)
                {
                    return new ValidationResult("For men's shirts, the size has to be greater or equal to 8.");
                }
                else if (shirt.Gender.Equals(GENDER_WOMEN, StringComparison.OrdinalIgnoreCase) && shirt.Size < 6)
                {
                    return new ValidationResult("For women's shirts, the size has to be greater or equal to 6.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
