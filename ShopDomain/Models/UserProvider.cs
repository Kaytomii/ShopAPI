using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDomain.Models;

public class UserProvider
{
    public int Id { get; set; }

    public Guid UserId { get; set; }
    public int ProviderId { get; set; }

    public string NumberProvider { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Provider Provider { get; set; } = null!;
}