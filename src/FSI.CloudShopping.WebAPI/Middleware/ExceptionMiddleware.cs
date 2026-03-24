namespace FSI.CloudShopping.WebAPI.Middleware;

using System.Net;
using System.Text.Json;
using FSI.CloudShopping.Domain.Core;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

/// <summary>
/// Middleware global de tratamento de exceções.
/// Garante respostas padronizadas para todos os tipos de erro.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorId = Guid.NewGuid().ToString("N")[..8].ToUpper();

        _logger.LogError(exception, "Error [{ErrorId}] on {Method} {Path}",
            errorId, context.Request.Method, context.Request.Path);

        var (statusCode, message, errors) = exception switch
        {
            DomainException domainEx => (
                HttpStatusCode.BadRequest,
                "Regra de negócio violada.",
                new[] { domainEx.Message }),

            ValidationException validationEx => (
                HttpStatusCode.UnprocessableEntity,
                "Dados inválidos.",
                validationEx.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToArray()),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Não autorizado.",
                new[] { "Autenticação necessária." }),

            KeyNotFoundException => (
                HttpStatusCode.NotFound,
                "Recurso não encontrado.",
                new[] { exception.Message }),

            _ => (
                HttpStatusCode.InternalServerError,
                "Erro interno. Por favor, tente novamente.",
                new[] { $"ID do erro: {errorId}" })
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            success = false,
            message,
            errors,
            errorId,
            timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
