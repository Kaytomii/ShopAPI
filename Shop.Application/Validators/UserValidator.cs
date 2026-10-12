using FluentValidation;
using Shop.Application.DTOs.UserDTOs;
using System.Text.RegularExpressions;

namespace Shop.Application.Validators;

public class UserValidator : AbstractValidator<UserCreateDTO>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters")
            .Must(password => Regex.IsMatch(password, @"[A-Z]"))
            .WithMessage("Password must contain at least one uppercase letter")
            .Must(password => Regex.IsMatch(password, @"[a-z]"))
            .WithMessage("Password must contain at least one lowercase letter")
            .Must(password => Regex.IsMatch(password, @"[0-9]"))
            .WithMessage("Password must contain at least one digit")
            .Must(password => Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            .WithMessage("Password must contain at least one special character");
    }
}
