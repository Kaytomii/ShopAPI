using AutoMapper;
using MediatR;
using Shop.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Commands.Category;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<ShopDomain.Models.Category>(request.Dto);
        var id = await _repository.AddCategoryAsync(category);
        return id ?? 0;
    }
}