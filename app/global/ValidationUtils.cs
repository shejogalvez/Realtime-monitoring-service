using System.ComponentModel.DataAnnotations;

namespace app.Validation
{
    public class ValidationUtils
    {
        public static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        Validator.TryValidateObject(
            model,
            context,
            results,
            validateAllProperties: true
        );

        return results;
    }
    }
}