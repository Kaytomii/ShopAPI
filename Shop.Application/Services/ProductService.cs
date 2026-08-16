using AutoMapper;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;

namespace Shop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ICachingService _cache;
    private readonly int _maxImages = 10;

    public ProductService(IProductRepository repo, IMapper mapper, ICachingService cache)
    {
        _repo = repo;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<int> CreateProductAsync(ProductCreateDTO dto)
    {
        if (dto.ImageUrls.Count > _maxImages)
            throw new Exception($"Max allowed images: {_maxImages}");

        var product = _mapper.Map<Product>(dto);

        product.Images = dto.ImageUrls
            .Select(url => new ProductImage { Url = url })
            .ToList();

        var id = await _repo.AddAsync(product);

        await _cache.RemoveAsync("products:all");
        await _cache.RemoveAsync($"product:{id}");

        return id;
    }

    public async Task<IEnumerable<ProductReadDTO>> GetAllAsync()
    {
        var cached = await _cache.GetAsync<IEnumerable<ProductReadDTO>>("products:all");
        if (cached != null)
            return cached;

        var products = await _repo.GetAllAsync();
        var dto = _mapper.Map<IEnumerable<ProductReadDTO>>(products);

        await _cache.SetAsync("products:all", dto, TimeSpan.FromMinutes(15));
        return dto;
    }

    public async Task<ProductReadDTO?> GetByIdAsync(int id)
    {
        var key = $"product:{id}";
        var cached = await _cache.GetAsync<ProductReadDTO>(key);
        if (cached != null)
            return cached;

        var product = await _repo.GetByIdAsync(id);
        if (product == null)
            return null;

        var dto = _mapper.Map<ProductReadDTO>(product);

        await _cache.SetAsync(key, dto, TimeSpan.FromMinutes(15));
        return dto;
    }
}
