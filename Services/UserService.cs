using server.Data;
using server.Models;

using System;
using Microsoft.EntityFrameworkCore;
using server.Exceptions;
using server.DTOs.Users;
namespace server.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly ILogger<UserService> _logger;

    public UserService(
        AppDbContext db,
        ILogger<UserService> logger)
    {
        _db = db;
        _logger = logger;
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = await _db.Users
            .AsNoTracking()
            .ToListAsync();

        return users
            .Select(MapToResponse)
            .ToList();
    }

    public async Task<UserResponse?> GetByIdAsync(int id)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new NotFoundException(
             $"User with ID {id} was not found"
            );
        }

        return MapToResponse(user);
    }

    public async Task<UserResponse> CreateAsync(
    CreateUserRequest request)
    {
        _logger.LogInformation(
            "Creating user with email {Email}",
            request.Email
        );

        var emailExists = await _db.Users
            .AnyAsync(x => x.Email == request.Email);

        if (emailExists)
        {
            throw new ConflictException(
                "A user with this email already exists.");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                request.Password),
            Role = "User"
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "User {UserId} created.",
            user.Id);

        return MapToResponse(user);
    }

    public async Task<UserResponse?> UpdateAsync(
    int id,
    UpdateUserRequest request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new NotFoundException(
                $"User with ID {id} was not found."
            );
        }

        var emailExists = await _db.Users
            .AnyAsync(x =>
                x.Email == request.Email &&
                x.Id != id);

        if (emailExists)
        {
            throw new ConflictException(
                "A user with this email already exists.");
        }

        await _db.SaveChangesAsync();

        return MapToResponse(user);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            throw new NotFoundException(
                $"User with ID {id} was not found.");
        }

        _db.Users.Remove(user);

        await _db.SaveChangesAsync();

        _logger.LogInformation(
            "User {UserId} deleted.",
            id);
    }

}
