using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTOs;
using server.Models;

namespace server.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(
        AppDbContext db, 
        IPasswordHasher<User> passwordHasher,
        IJwtService jwtService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var emailExists = await _db.Users
            .AnyAsync(u => u.Email == request.Email);

        if(emailExists)
        {
            throw new InvalidOperationException(
                "Email is already registered."
            );
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        _db.Users.Add(user);

        await _db.SaveChangesAsync();
    }

    public async Task<LoginResponse?> LoginAsync(
        LoginRequest request
    )
    {
        var user = await _db.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email
        );

        if(user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if(result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return new LoginResponse
        {
            Token = _jwtService.GenerateToken(user)
        };
    }
}
