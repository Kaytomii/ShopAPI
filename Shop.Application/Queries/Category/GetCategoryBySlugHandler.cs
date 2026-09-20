using AutoMapper;
using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Queries.Category;

public class GetCategoryBySlugHandler : IRequestHandler<GetCategoryBySlugQuery, CategoryReadDTO?>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoryBySlugHandler(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryReadDTO?> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetBySlugAsync(request.Slug);
        return category == null ? null : _mapper.Map<CategoryReadDTO>(category);
    }
}