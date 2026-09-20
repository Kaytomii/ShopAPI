using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDomain.Models;

public class UserAddress
{
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string House { get; set; } = string.Empty;
    public string Apartment { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}