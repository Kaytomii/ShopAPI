using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.UserDTOs;

public class ExternalAuthDTO
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string ProviderUserId { get; set; } = string.Empty;
}