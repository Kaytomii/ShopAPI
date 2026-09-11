using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.ProductDTOs;

public class ProductFeedbackDto
{
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? Rating { get; set; }
}