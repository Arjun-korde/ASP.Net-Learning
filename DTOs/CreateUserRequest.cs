using System;
using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class CreateUserRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

}
