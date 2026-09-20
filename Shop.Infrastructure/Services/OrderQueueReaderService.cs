using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shop.Application.DTOs.OrdersDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Configuration;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Shop.Infrastructure.Services;

public class OrderQueueReaderService : BackgroundService
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IProductRepository _productRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IEmailService _emailService;

    public OrderQueueReaderService(
        IProductRepository productRepo,
        IOrderRepository orderRepo,
        IEmailService emailService,
        IOptions<RabbitMqSettings> rabbitMqOptions)
    {
        _rabbitMqSettings = rabbitMqOptions.Value;
        _productRepo = productRepo;
        _orderRepo = orderRepo;
        _emailService = emailService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.CompletedTask;
    }

    private async Task ProcessOrderAsync(OrderCreateDto dto)
    {
        var items = new List<OrderDetail>();
        bool allAvailable = true;
        decimal total = 0;

        foreach (var p in dto.Products)
        {
            var product = await _productRepo.GetByIdAsync(p.ProductId);

            if (product == null || product.StockQty < p.Count)
            {
                allAvailable = false;
                continue;
            }

            items.Add(new OrderDetail
            {
                ProductId = Guid.Parse(product.Id.ToString()),
                Price = product.Price,
                Count = p.Count
            });

            total += product.Price * p.Count;
        }

        if (allAvailable)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                Paid = false,
                Status = "Pending",
                OrderDetails = items
            };

            await _orderRepo.CreateOrderAsync(order);

            await _emailService.SendOrderCreatedEmailAsync(dto.UserId, items, total);
        }
        else
        {
            await _emailService.SendOrderWaitingEmailAsync(dto.UserId);
        }
    }
}
