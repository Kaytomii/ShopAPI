using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ShopDomain.Models;

public class User
{
    public int id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string HashPassword { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
