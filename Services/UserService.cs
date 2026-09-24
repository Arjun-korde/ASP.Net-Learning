using server.Data;
using server.DTOs;
using server.Models;

using System;
using Microsoft.EntityFrameworkCore;
using server.Services;

namespace server.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<UserResponse>> GetUsersAsync()
    {
        var users = await _db.Users.ToListAsync();

        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        }).ToList();
    }

    public async Task<UserResponse?> GetUserAsync(int id)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return null;
        }

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponse> CreateUserAsync(
    CreateUserRequest request)
    {
        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        _db.Users.Add(user);

        await _db.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponse?> UpdateUserAsync(
    int id,
    UpdateUserRequest request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return null;
        }

        user.Name = request.Name;
        user.Email = request.Email;

        await _db.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
        {
            return false;
        }

        _db.Users.Remove(user);

        await _db.SaveChangesAsync();

        return true;
    }

}
