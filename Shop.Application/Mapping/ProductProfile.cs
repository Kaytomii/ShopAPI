using AutoMapper;
using Shop.Application.DTOs.ProductDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateDTO, Product>();

        CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.Images,
                opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()));
    }
}