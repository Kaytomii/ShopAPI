using System;
using System.Collections.Generic;
using System.Text;

namespace ShopDomain.Models;

public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<UserProvider> UserProviders { get; set; } = new List<UserProvider>();
}