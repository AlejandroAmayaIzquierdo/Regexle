using System.Net;
using Newtonsoft.Json;

namespace WebServer.Middlewares;

public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger
)
{
    private readonly RequestDelegate _next = next;

    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var problemDetails = new
            {
                type = "about:blank",
                title = "Internal Server Error",
                status = context.Response.StatusCode,
                detail = ex.Message,
                instance = context.Request.Path,
            };
            var result = JsonConvert.SerializeObject(problemDetails);
            await context.Response.WriteAsync(result);

            _logger.LogError(
                ex,
                "An unhandled exception has occurred while executing the request."
            );
        }
    }
}
