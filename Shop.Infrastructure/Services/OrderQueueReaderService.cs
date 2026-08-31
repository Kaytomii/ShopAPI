using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shop.Application.DTOs.OrdersDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Services;

public class OrderQueueReaderService : BackgroundService
{
    private readonly ConnectionFactory _factory;
    private readonly IProductRepository _productRepo;
    private readonly IOrderRepository _orderRepo;
    private readonly IEmailService _emailService;

    public OrderQueueReaderService(
        IProductRepository productRepo,
        IOrderRepository orderRepo,
        IEmailService emailService)
    {
        _factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        _productRepo = productRepo;
        _orderRepo = orderRepo;
        _emailService = emailService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare("Orders", durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (sender, args) =>
        {
            var json = Encoding.UTF8.GetString(args.Body.ToArray());
            var dto = JsonSerializer.Deserialize<OrderCreateDto>(json);

            await ProcessOrderAsync(dto);
        };

        channel.BasicConsume("Orders", autoAck: true, consumer);

        return Task.CompletedTask;
    }

    private async Task ProcessOrderAsync(OrderCreateDto dto)
    {
        var items = new List<OrderDetail>();
        bool allAvailable = true;
        decimal total = 0;

        foreach (var p in dto.Products)
        {
            var product = await _productRepo.GetByIdAsync(p.ProductId);

            if (product == null || product.Count < p.Count)
            {
                allAvailable = false;
                continue;
            }

            items.Add(new OrderDetail
            {
                ProductId = p.ProductId,
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