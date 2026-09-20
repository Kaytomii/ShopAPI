using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Queries.Category;

public record GetCategoryBySlugQuery(string Slug) : IRequest<CategoryReadDTO?>;