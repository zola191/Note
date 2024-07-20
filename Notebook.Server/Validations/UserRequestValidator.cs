using FluentValidation;
using Notebook.Server.Dto;
using Notebook.Server.Validations.CustomValidators;

namespace Notebook.Server.Validators
{
    public class UserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(f => f.Email)
                .NotNull().NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(f => f.Password).ValidatePassowrd();
            RuleFor(f => f.ConfirmPassword).Matches(f=>f.Password);
        }
    }
}
