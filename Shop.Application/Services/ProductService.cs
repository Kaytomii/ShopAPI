using AutoMapper;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly int _maxImages;

    public ProductService(IProductRepository repo, IMapper mapper, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
        _maxImages = config.GetValue<int>("ProductImages:MaxCount");
    }

    public async Task<int> CreateProductAsync(ProductCreateDTO dto)
    {
        if (dto.ImageUrls.Count > _maxImages)
            throw new Exception($"Max allowed images: {_maxImages}");

        var product = _mapper.Map<Product>(dto);

        product.Images = dto.ImageUrls
            .Select(url => new ProductImage { Url = url })
            .ToList();

        return await _repo.AddAsync(product);
    }

    public async Task<IEnumerable<ProductReadDTO>> GetAllAsync()
    {
        var products = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductReadDTO>>(products);
    }

    public async Task<ProductReadDTO?> GetByIdAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductReadDTO>(product);
    }
}
