using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.OrdersDTOs;

public class OrderProductDto
{
    public Guid ProductId { get; set; }
    public int Count { get; set; }
}