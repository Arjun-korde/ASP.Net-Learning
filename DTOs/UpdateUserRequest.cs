using System;
using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class UpdateUserRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
}
