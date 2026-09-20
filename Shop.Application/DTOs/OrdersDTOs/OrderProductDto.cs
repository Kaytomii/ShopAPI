using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.OrdersDTOs;

public class OrderProductDto
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}