using FluentValidation;
using Shop.Application.DTOs.ProductDTOs;

namespace Shop.Application.Validators;

public class ProductValidator : AbstractValidator<ProductCreateDTO>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Product description is required")
            .MaximumLength(1000).WithMessage("Product description cannot exceed 1000 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than 0")
            .LessThanOrEqualTo(999999.99m).WithMessage("Product price cannot exceed 999999.99");

        RuleFor(p => p.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be a positive number");

        RuleFor(p => p.ImageUrls)
            .NotNull().WithMessage("ImageUrls cannot be null")
            .Must(urls => urls.Count > 0).WithMessage("At least one image URL is required")
            .Must(urls => urls.All(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)))
            .WithMessage("All image URLs must be valid URIs");
    }
}
