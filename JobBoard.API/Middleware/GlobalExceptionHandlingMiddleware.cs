using JobBoard.API.Contracts.Errors;
using JobBoard.Application.Common.Exceptions;
using System.Diagnostics;
using System.Text.Json;

namespace JobBoard.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            catch(Exception ex) 
            {
                _logger.LogError(ex, $"Unhandled exception occurred. TraceId: {context.TraceIdentifier}");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var title = exception switch
            {
                BadRequestException => "Bad Request",
                UnauthorizedException => "Unauthorized",
                NotFoundException => "Resource not found",
                _ => "An unexpected error occurred"
            };

            var response = new ApiErrorResponse
            {
                TraceId = context.TraceIdentifier,
                Status = statusCode,
                Title = title,
                Detail = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
