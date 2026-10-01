using System;

namespace server.DTOs.Users;

public class UserQuery
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? Search { get; set; }

    public string? Role { get; set; }

    public string? SortBy { get; set; }

    public string SortOrder { get; set; } = "asc";
}
