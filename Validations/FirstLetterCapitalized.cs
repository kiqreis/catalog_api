using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validations;

public class FirstLetterCapitalized : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var firstLetter = value.ToString()?[0].ToString();

        return firstLetter != firstLetter?.ToUpper()
            ? new ValidationResult("The first letter must be capitalized")
            : ValidationResult.Success;
    }
}