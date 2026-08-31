using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.OrdersDTOs;

public class OrderCreateDto
{
    public Guid UserId { get; set; }
    public List<OrderProductDto> Products { get; set; } = new();
}
