using server.Data;
using server.Models;

using System;
using Microsoft.EntityFrameworkCore;
using server.Exceptions;
using server.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using server.DTOs.Common;
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

    public async Task<PagedResult<UserResponse>> GetAllAsync(UserQuery query)
    {
        var page = Math.Max(query.Page, 1);

        var pageSize = Math.Clamp(
            query.PageSize,
            1,
            100
        );

        var baseQuery = _db.Users.AsNoTracking();

        if(!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            baseQuery = baseQuery.Where( u =>
                u.Name.Contains(search) ||
                u.Email.Contains(search));
        }

        if(!string.IsNullOrWhiteSpace(query.Role))
        {
            baseQuery = baseQuery.Where( 
                u => u.Role == query.Role
            );
        }

        baseQuery = query.SortBy?.ToLower() switch
        {
            "name" => 
                query.SortOrder == "desc"
                ? baseQuery.OrderByDescending(u => u.Name)
                : baseQuery.OrderBy(u => u.Name),

                "createdat" => 
                    query.SortOrder == "desc"
                    ? baseQuery.OrderByDescending(u => u.CreatedAt)
                    : baseQuery.OrderBy(u => u.CreatedAt),

                _ => 
                    baseQuery.OrderBy( u => u.Id)
        };

        var totalCount = await baseQuery.CountAsync();

        var users = await baseQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<UserResponse>
        {
            Items = users
            .Select(MapToResponse)
            .ToList(),

            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
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
