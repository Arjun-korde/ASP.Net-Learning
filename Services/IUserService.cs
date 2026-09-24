using server.DTOs;

namespace server.Services;

public interface IUserService
{
    Task<List<UserResponse>> GetUsersAsync();

    Task<UserResponse?> GetUserAsync(int id);

    Task<UserResponse> CreateUserAsync(
        CreateUserRequest request);

    Task<UserResponse?> UpdateUserAsync(
        int id,
        UpdateUserRequest request);

    Task<bool> DeleteUserAsync(int id);
}