using server.DTOs.Common;
using server.DTOs.Users;

namespace server.Services;

public interface IUserService
{
    Task<PagedResult<UserResponse>> GetAllAsync(UserQuery query);

    Task<UserResponse?> GetByIdAsync(int id);

    Task<UserResponse> CreateAsync(
        CreateUserRequest request);

    Task<UserResponse?> UpdateAsync(
        int id,
        UpdateUserRequest request);

    Task DeleteAsync(int id);
}