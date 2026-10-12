using FluentValidation;
using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Validators;

public class CategoryValidator : AbstractValidator<CategoryCreateDTO>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters");

        RuleFor(c => c.Slug)
            .NotEmpty().WithMessage("Slug is required")
            .Matches(@"^[a-zA-Z0-9-]+$").WithMessage("Slug can contain only letters, numbers, and hyphens")
            .MaximumLength(100).WithMessage("Slug cannot exceed 100 characters");

        RuleFor(c => c.ParentId)
            .Must(id => id == null || id > 0)
            .WithMessage("ParentId must be a positive number");
    }
}
