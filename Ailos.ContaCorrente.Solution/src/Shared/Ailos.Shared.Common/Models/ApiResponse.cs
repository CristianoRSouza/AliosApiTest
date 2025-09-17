using Ailos.Shared.Common.Enums;

namespace Ailos.Shared.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public ErrorType? ErrorType { get; set; }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public ErrorType? ErrorType { get; set; }
}
