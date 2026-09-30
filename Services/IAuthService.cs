using System;
using server.DTOs.Auth;

namespace server.Services;

public interface IAuthService
{
    Task RegisterAsync(RegisterRequest request);

    Task<LoginResponse?> LoginAsync(LoginRequest request);
}
