﻿using FluentValidation;
using Notebook.Server.Domain;

namespace Notebook.Server.Validations
{
    public class NoteFromFileValidator : AbstractValidator<Note>
    {
        public NoteFromFileValidator()
        {
            RuleFor(f => f.FirstName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4).WithMessage("minimum length limited to 4 symbols")
                .MaximumLength(25).WithMessage("maximum length limited to 25 symbols");

            RuleFor(f => f.MiddleName)
                .MinimumLength(4)
                .MaximumLength(25);

            RuleFor(f => f.LastName)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(25);

            RuleFor(f => f.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .Matches(@"^\+?[0-9]{3,13}$")
                .WithMessage("Invalid phonenumber");

            RuleFor(f => f.Country)
                .NotNull()
                .NotEmpty()
                .MinimumLength(4).WithMessage("minimum length limited to 4 symbols")
                .MaximumLength(50).WithMessage("maximum length limited to 25 symbols");

            RuleFor(f => f.Organization)
                .MinimumLength(2).WithMessage("minimum length limited to 2 symbols")
                .MaximumLength(25).WithMessage("maximum length limited to 25 symbols");

            RuleFor(f => f.Position)
                .MinimumLength(2).WithMessage("minimum length limited to 2 symbols")
                .MaximumLength(25).WithMessage("maximum length limited to 25 symbols");

            RuleFor(f => f.Other)
                .MaximumLength(100)
                .WithMessage("maximum length limited to 100 symbols");
        }
    }
}
