using System;

namespace LMS.Common.Responses;

public record ApiResponse<T>(
    bool Success,
    T? Data,
    string? Message = null,
    int? StatusCode = 200
);
