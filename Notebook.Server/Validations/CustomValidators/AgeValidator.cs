using FluentValidation;

namespace Notebook.Server.Validations.CustomValidators
{
    public static class AgeValidator
    {
        public static IRuleBuilderOptions<T, DateTime> Adult<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            var currentDate = DateTime.UtcNow.Year;

            return ruleBuilder.Must(f => f.Year > currentDate);
        }
    }
}
