namespace FSI.CloudShopping.WebAPI.Controllers;

using FSI.CloudShopping.Application.Common;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result switch
        {
            Result<T>.Success success => Ok(new ApiResponse<T>(true, success.Value)),
            Result<T>.Failure failure => BadRequest(new ApiResponse<T>(false, default, failure.Errors)),
            _ => StatusCode(500, new ApiResponse<T>(false, default, ["Unexpected error"]))
        };
    }

    protected IActionResult HandleResultCreated<T>(Result<T> result, string routeName, object routeValues)
    {
        return result switch
        {
            Result<T>.Success success => CreatedAtRoute(routeName, routeValues, new ApiResponse<T>(true, success.Value)),
            Result<T>.Failure failure => BadRequest(new ApiResponse<T>(false, default, failure.Errors)),
            _ => StatusCode(500, new ApiResponse<T>(false, default, ["Unexpected error"]))
        };
    }

    protected IActionResult HandleNotFound<T>(Result<T>? result, string notFoundMessage = "Resource not found")
    {
        if (result == null)
            return NotFound(new ApiResponse<T>(false, default, [notFoundMessage]));

        return result switch
        {
            Result<T>.Success success => Ok(new ApiResponse<T>(true, success.Value)),
            Result<T>.Failure failure => NotFound(new ApiResponse<T>(false, default, failure.Errors)),
            _ => StatusCode(500, new ApiResponse<T>(false, default, ["Unexpected error"]))
        };
    }

    protected Guid GetCurrentCustomerId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst("customerId");
        return claim != null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
    }

    protected string GetCurrentUserEmail()
    {
        return User.FindFirst("email")?.Value ?? string.Empty;
    }

    protected string GetCurrentUserRole()
    {
        return User.FindFirst("role")?.Value ?? string.Empty;
    }
}

public class ApiResponse<T>
{
    public bool Success { get; }
    public T? Data { get; }
    public IReadOnlyList<string> Errors { get; }

    public ApiResponse(bool success, T? data = default, IReadOnlyList<string>? errors = null)
    {
        Success = success;
        Data = data;
        Errors = errors ?? [];
    }
}
