using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Commands.Category;

public record CreateCategoryCommand(CategoryCreateDTO Dto) : IRequest<int>;