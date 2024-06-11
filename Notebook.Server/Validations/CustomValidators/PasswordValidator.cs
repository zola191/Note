using FluentValidation;
using System.Text.RegularExpressions;

namespace Notebook.Server.Validations.CustomValidators
{
    public static class PasswordValidator
    {
        public static IRuleBuilderOptions<T, string> ValidatePassowrd<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            var lowercase = new Regex("[a-z]+");
            var uppercase = new Regex("[A-Z]+");
            var number = new Regex("[0-9]+");
            var symbol = new Regex("[\\!\\?\\*\\.]+");

            return ruleBuilder
                .MinimumLength(6).WithMessage("Your password length must be at least 6.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(uppercase).WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(lowercase).WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(number).WithMessage("Your password must contain at least one number.")
                .Matches(symbol).WithMessage("Your password must contain at least one (!? *.).");
        }
    }
}
