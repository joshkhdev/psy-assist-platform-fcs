using PsyAssistFeedback.Application.Exceptions;
using Serilog;

namespace PsyAssistFeedback.WebApi.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            Log.Error(ex, ex.Message);

            context.Response.StatusCode = 404;
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync(ex.Message);
        }
        catch (IncorrectDataException ex)
        {
            Log.Error(ex, ex.Message);

            context.Response.StatusCode = 400;
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync(ex.Message);
        }
        catch (BusinessLogicException ex)
        {
            Log.Error(ex, ex.Message);

            context.Response.StatusCode = 422;
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync(ex.Message);
        }
        catch (InternalPlatformErrorException ex)
        {
            Log.Error(ex, ex.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync("An error occurred. Please try again later.");
        }
    }
}