using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopDomain.Models;

public class ProductFeedback
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }

    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public int? Rating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}