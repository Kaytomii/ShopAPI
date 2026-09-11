using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.UserDTOs;

public class UserQueueDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
