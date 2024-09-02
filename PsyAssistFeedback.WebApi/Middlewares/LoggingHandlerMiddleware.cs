using Serilog;
using System.Security.Claims;

namespace PsyAssistPlatform.WebApi.Middlewares;

public class LoggingHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var userId = Guid.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

        var requestBody = await ReadRequestBodyAsync(context);

        Log.Information("REQUEST: UserId: {@UserId} | RequestMethod: {@RequestMethod} | RequestPath: {@RequestPath} | RequestBody: {@RequestBody}", userId, context.Request.Method, context.Request.Path, requestBody);

        await _next(context);
    }

    private async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        context.Request.EnableBuffering();
        var memoryStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var reader = new StreamReader(memoryStream);
        var requestBody = await reader.ReadToEndAsync();

        context.Request.Body.Seek(0, SeekOrigin.Begin);

        return requestBody;
    }
}
