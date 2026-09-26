using System;
using server.DTOs;

namespace server.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
