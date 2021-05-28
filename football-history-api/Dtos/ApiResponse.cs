namespace football.history.api.Controllers
{
    public record ApiResponse<T> (T Result, ApiError? Error = null);
    
    public record ApiError(string? Message = null, string? Code = "UNKNOWN_ERROR");
}
