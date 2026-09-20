using AutoMapper;
using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Queries.Category;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryReadDTO?>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CategoryReadDTO?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.Id);
        return category == null ? null : _mapper.Map<CategoryReadDTO>(category);
    }
}