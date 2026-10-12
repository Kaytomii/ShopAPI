using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Queries.Product;
using ShopApp.Controllers;
using Shop.Application.Interfaces.Services;
using ShopApi.Interfaces;
using Shop.Infrastructure.Services;
using Shop.Infrastructure.Data;

namespace Shop.Test;

public class ProductControllerTests
{
    [Fact]
    public async Task GetById_WithValidId_ReturnsOk()
    {
        var mockMediator = new Mock<IMediator>();
        var mockImageService = new Mock<IImageService>();
        var mockProductService = new Mock<Shop.Application.Interfaces.Services.IProductService>();
        var mockDbContext = new Mock<ShopDbContext>(new Microsoft.EntityFrameworkCore.DbContextOptions<ShopDbContext>());

        var productDto = new ProductReadDTO
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100m,
            CategoryId = 1,
        };

        mockMediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(productDto);

        var mockMongoDb = new Mock<MongoDbService>();
        var feedbackService = new ProductFeedbackService(mockMongoDb.Object);
        var controller = new ProductController(mockProductService.Object, mockImageService.Object, mockMediator.Object, feedbackService);

        var result = await controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(productDto, okResult.Value);
        mockMediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ReturnsNotFound()
    {
        var mockMediator = new Mock<IMediator>();
        var mockImageService = new Mock<IImageService>();
        var mockProductService = new Mock<Shop.Application.Interfaces.Services.IProductService>();
        var mockDbContext = new Mock<ShopDbContext>(new Microsoft.EntityFrameworkCore.DbContextOptions<ShopDbContext>());

        mockMediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProductReadDTO)null);

        var mockMongoDb = new Mock<MongoDbService>();
        var feedbackService = new ProductFeedbackService(mockMongoDb.Object);
        var controller = new ProductController(mockProductService.Object, mockImageService.Object, mockMediator.Object, feedbackService);

        var result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
        mockMediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}




