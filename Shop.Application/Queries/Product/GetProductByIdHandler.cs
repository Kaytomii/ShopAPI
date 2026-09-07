using AutoMapper;
using MediatR;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Queries.Product;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductReadDTO?>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    public GetProductByIdHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ProductReadDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetByIdAsync(request.id);
        return _mapper.Map<ProductReadDTO>(entities);

    }
}
