using System;

namespace server.DTOs.Auth;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
}
