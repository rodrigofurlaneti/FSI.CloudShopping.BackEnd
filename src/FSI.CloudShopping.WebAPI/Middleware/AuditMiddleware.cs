namespace FSI.CloudShopping.WebAPI.Middleware;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Middleware de auditoria — registra operações de escrita (POST, PUT, PATCH, DELETE).
/// </summary>
public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly HashSet<string> WriteMethods =
        new(StringComparer.OrdinalIgnoreCase) { "POST", "PUT", "PATCH", "DELETE" };

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        if (!WriteMethods.Contains(context.Request.Method))
            return;

        try
        {
            var userId = context.User?.FindFirst("sub")?.Value
                       ?? context.User?.FindFirst("customerId")?.Value
                       ?? "anonymous";
            var userEmail = context.User?.FindFirst("email")?.Value ?? "anonymous";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var path = context.Request.Path.Value ?? "/";
            var method = context.Request.Method;

            var auditLog = AuditLog.Create(
                entityName: "HttpRequest",
                entityId: path,
                action: method,
                oldValues: null,
                newValues: $"{{\"path\":\"{path}\",\"status\":{context.Response.StatusCode}}}",
                userId: userId,
                userEmail: userEmail,
                ipAddress: ipAddress,
                userAgent: userAgent);

            var repo = context.RequestServices.GetService<IAuditLogRepository>();
            if (repo != null)
            {
                await repo.AddAsync(auditLog);
                await repo.SaveChangesAsync();
            }
        }
        catch
        {
            // Audit failure must never break the main request flow
        }
    }
}
